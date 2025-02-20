using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Arch.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Angular;
using Stimulsoft.Report.Mvc;//does not exist in the other
using Stimulsoft.Report.Web;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Abp.Application.Services.Dto;
using Arch.Domain.FinancialReports.Dto;
using MimeKit;

//using NPOI.SS.Formula.Functions;
using Stimulsoft.Report.Dictionary;
using Arch.Reports;
using Microsoft.Data.SqlClient;
//using NPOI.HPSF;
using System.IO;
//using Arch.OrganizationalUnits;
using Abp.Authorization.Users;
//using Arch.Domain.OrganizationalUnits;
using Abp.Runtime.Session;
using Abp.Extensions;
using Stripe.Reporting;
using Abp.Organizations;

namespace Arch.Web.Controllers
{

    public class ViewerController : ArchControllerBase
    {

        private string rptname;
        private string connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IConfigurationRoot _appConfiguration;
        protected OrganizationUnitManager _OrganizationalUnitManager;


        //      static ViewerController()
        //      {
        // How to Activate
        //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnO...";
        //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
        //Stimulsoft.Base.StiLicense.LoadFromStream(stream);
        //        }

        public ViewerController(
            IWebHostEnvironment webHostEnvironment,
            IAppConfigurationAccessor appConfigurationAccessor, OrganizationUnitManager OrganizationalUnitManager)
        {

            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk5HpBdjBv409VLMtz2HLQLe/rVHrGMiPs1JwzUddxN854SLn" +
"qJ1G/WshVabcIWw+0sB60zY/4aMid3NNh2lRWKZKQk96oO+4VwSM6v/okq0+MFt1ThfmE8yONhsaMIyPXufnDGYMpH" +
"vO+mDYFHxLNaAbATV5dGVpsYumMgmQ0lLaHW1J3mrpjb34ikQA5dHlqydqfbkvRfS/Ls7H+QBYU6ZRE7ABulFdllsA" +
"t5L/svpvuv1gKoDKPB1P8zb8tM8O68Hty3b/Bw4DzWBDJuqJGR9jA5/jdPqKmqNdJ8I+UtodIsfzqdt+UezM9Z1rV/" +
"Cc3AFhFuALIdMFUZ4BN/NNLXSJl+RqU0GxzoNOb+99V/+qrii+Glhgwsk9duAU/hTIDOmPXemAwffseEAqqu7rP2dB" +
"w8aXe5/idaF2kZZoNBRqiYDhqEMZtrTRrs1zVqg9/ZHifhCjkxNeUuuGPv4VmkflP0mkQJi3OoHukyEgUoC6vSLqjJ" +
"9IaLTfMMKbiqV7ZoR02UvgIn2H8ZnyRbXlqFzM335/DCvjk20jPkM8Xquw==";

            _webHostEnvironment = webHostEnvironment;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _OrganizationalUnitManager = OrganizationalUnitManager;
        }

        [DisableAuditing]

        [HttpPost]
        //  public IActionResult InitViewer(string id = "0")
        public IActionResult InitViewer()
        {

            //  rptname = id;
            var requestParams = StiAngularViewer.GetRequestParams(this);

            var options = new StiAngularViewerOptions();
            options.Actions.GetReport = "GetReport";
            options.Actions.ViewerEvent = "ViewerEvent";
            options.Actions.Interaction = "ViewerInteraction"; //this allows interaction with the viewer...check if you really want this
            options.Appearance.ScrollbarsMode = true;
            options.Toolbar.ShowDesignButton = true;
            options.Toolbar.ShowParametersButton = false;
            options.Server.RequestTimeout = 120;



            return StiAngularViewer.ViewerDataResult(requestParams, options);
        }

        [HttpPost]
        //    public IActionResult GetReport(string id = "0",string parameterlist = null)
        public IActionResult GetReport()
        {


            string id = "0";
            string parameterlist = null;
            string reporttype = null;

            var httpContext = new Stimulsoft.System.Web.HttpContext(this.HttpContext);
            var properties = httpContext.Request.Params["properties"]?.ToString();
            if (properties != null)
            {
                var data = Convert.FromBase64String(properties);
                var json = Encoding.UTF8.GetString(data);
                JContainer container = JsonConvert.DeserializeObject<JContainer>(json);
                foreach (JToken token in container.Children())
                {
                    if (((JProperty)token).Name == "id")
                    {
                        id = ((JProperty)token).Value.Value<string>();
                    }
                    if (((JProperty)token).Name == "parameterlist")
                    {
                        parameterlist = ((JProperty)token).Value.Value<string>();
                    }
                    if (((JProperty)token).Name == "reporttype")
                    {
                        reporttype = ((JProperty)token).Value.Value<string>();
                    }
                }
            }

            if (id == null || id == "0")
                id = "TwoSimpleLists";

            id = id.Replace("*", "/");

            var testDashboard = id.Split('/');




            StiReport report = new StiReport();

            if ((testDashboard == null || !testDashboard[0].ToLower().Contains("dashboard")) && !(reporttype != null && reporttype.ToLower().Contains("dashboard")))
            {
                report = StiReport.CreateNewReport();
            }
            else
            {
                // Create new dashboard
                report = StiReport.CreateNewDashboard();
            }




            var path = GetRptPath(id);// StiAngularHelper.MapPath(this, $"Reports/MasterDetail.mrt");
            report.Load(path);

            connectionString = (_appConfiguration.GetConnectionString(
                Arch.ArchConsts.ConnectionStringName
            ));



            if (parameterlist != null)
            {
                FinancialReportDto[] paramlist = JsonConvert.DeserializeObject<FinancialReportDto[]>(parameterlist);

                foreach (var param in paramlist)
                {

                    if (param.ParamType.ToLower().Contains("date"))


                        report[param.ParamName] = param.ParamValue.Trim().Length <= 19 ? param.ParamValue.Trim() : param.ParamValue.Trim().Left(19);

                    else if (param.ParamType.ToLower().Contains("int"))
                        report[param.ParamName] = Int32.Parse(param.ParamValue);
                    else if (param.ParamType.ToLower().Contains("bool"))
                        report[param.ParamName] = param.ParamValue != null && Convert.ToBoolean(param.ParamValue);
                    else if (param.ParamType.ToLower().Contains("string"))
                        report[param.ParamName] = param.ParamValue ?? string.Empty;

                    else if (param.ParamType.ToLower().Contains("default"))
                    {
                        if (param.ParamValue.ToLower().Contains("t"))
                            report["@TenantId"] = AbpSession.TenantId;

                        if (param.ParamValue.ToLower().Contains("o"))
                            report["@OrganizationalUnitId"] = "00001";//_OrganizationalUnitManager.GetCurrentOrganizationalUnitId();

                        if (param.ParamValue.ToLower().Contains("i"))
                            report["@IsDeleted"] = 0;


                    }
                    else
                        report[param.ParamName] = param.ParamValue ?? string.Empty;



                }

            }

            //  ((StiSqlDatabase)report.Dictionary.Databases.["Enterprise"]).ConnectionString = connectionString;
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new StiSqlDatabase("Enterprise", "Enterprise", connectionString, false));

            int dbtablescount = report.Dictionary.DataSources.Count;
            // ((StiSqlSource)report.Dictionary.DataSources["DataSourceName"]).SqlCommand = "select * from Table where Column = 100";
            for (int i = 0; i < dbtablescount; i++)
            {
                ((StiSqlSource)report.Dictionary.DataSources[i]).CommandTimeout = 300000;
            }
            //  ((StiSqlSource)report.Dictionary.DataSources["Enterprise"]).CommandTimeout = 300000;
            //   report.Dictionary.Databases.Clear();

            //   report.Dictionary.Databases.Add(new StiSqlDatabase("Enterprise", connectionString));

            report.Dictionary.Synchronize();
            report.Compile();


            return StiAngularViewer.GetReportResult(this, report);
        }

        [HttpPost]
        public IActionResult ViewerInteraction()

        {

            RouteValueDictionary routeValues = StiAngularViewer.GetRouteValues(this);



            return StiAngularViewer.InteractionResult(this);

        }
        [HttpPost]
        public IActionResult ViewerEvent()
        {
            return StiAngularViewer.ViewerEventResult(this);
        }



        public string GetRptPath(string id)
        {
            //   organizationationalunitid = "0";
            string rr;
            try
            {


                if (id.Length <= 200)
                {
                    //    id1 = "0";
                    var id7 = "";
                }
                else
                {
                    string[] rptfull = Encoding.UTF8.GetString(Convert.FromBase64String(id)).Split("@@@");


                    if (rptfull.Length >= 3)
                    {
                        // var rptfull = "" ;


                        var rptreal = rptfull[0]; // rptfull.Substring(0, rptfull.Length - 8);
                        var rpttemp1 = rptfull[1];
                        var rptnow = DateTime.Now.ToString("yyyyMMdd");
                        var t = rptfull[2];
                        if (rptnow != rpttemp1 || rpttemp1.Length != 8)
                            id = "0";
                        else
                            id = rptreal;
                    }
                    else
                        id = "0";
                }
            }
            catch (Exception ex)
            {
                rr = "";
            }

            string reportpath = Path.Combine(this._webHostEnvironment.ContentRootPath, "Reports/" + id + ".mrt");


            //  string reportpath = StiAngularHelper.MapPath(this, "Reports/" + id + ".mrt");

            return reportpath;

        }
    }
}


