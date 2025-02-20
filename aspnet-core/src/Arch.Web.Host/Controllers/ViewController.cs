using Abp.Organizations;
using Arch.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using Tweetinvi.Core.Models;

namespace Arch.Web.Controllers
{
 
    public class ViewController : ArchControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private string rptname;
        private string connectionString;
      
        readonly IConfigurationRoot _appConfiguration;
        protected OrganizationUnitManager _OrganizationalUnitManager;


        //static ViewController()
        //{
        //    // How to Activate
        //    //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnO...";
        //    //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
        //    //Stimulsoft.Base.StiLicense.LoadFromStream(stream);
          

        //}

        public ViewController(IWebHostEnvironment webHostEnvironment, IAppConfigurationAccessor appConfigurationAccessor, OrganizationUnitManager OrganizationalUnitManager)
        {

            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk5HpBdjBv409VLMtz2HLQLe/rVHrGMiPs1JwzUddxN854SLn" +
"qJ1G/WshVabcIWw+0sB60zY/4aMid3NNh2lRWKZKQk96oO+4VwSM6v/okq0+MFt1ThfmE8yONhsaMIyPXufnDGYMpH" +
"vO+mDYFHxLNaAbATV5dGVpsYumMgmQ0lLaHW1J3mrpjb34ikQA5dHlqydqfbkvRfS/Ls7H+QBYU6ZRE7ABulFdllsA" +
"t5L/svpvuv1gKoDKPB1P8zb8tM8O68Hty3b/Bw4DzWBDJuqJGR9jA5/jdPqKmqNdJ8I+UtodIsfzqdt+UezM9Z1rV/" +
"Cc3AFhFuALIdMFUZ4BN/NNLXSJl+RqU0GxzoNOb+99V/+qrii+Glhgwsk9duAU/hTIDOmPXemAwffseEAqqu7rP2dB" +
"w8aXe5/idaF2kZZoNBRqiYDhqEMZtrTRrs1zVqg9/ZHifhCjkxNeUuuGPv4VmkflP0mkQJi3OoHukyEgUoC6vSLqjJ" +
"9IaLTfMMKbiqV7ZoR02UvgIn2H8ZnyRbXlqFzM335/DCvjk20jPkM8Xquw==";

            _appConfiguration = appConfigurationAccessor.Configuration;
            _OrganizationalUnitManager = OrganizationalUnitManager;

            _webHostEnvironment = webHostEnvironment;
        }

        public string Index()
        {
            return "This is my default action...";
        }

        //
        // GET : /HelloWorld/Welcome/
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        public IActionResult Dashboards()
        {
            var dashboardFiles = Directory.GetFiles($"{_webHostEnvironment.ContentRootPath}/Dashboards", "*.mrt");
            var fileNames = new string[dashboardFiles.Length];
            var index = 0;
            foreach (var filePath in dashboardFiles)
            {
                fileNames[index++] = Path.GetFileNameWithoutExtension(filePath);
            }

            ViewBag.FileNames = fileNames;

            var fileName = RouteData.Values["id"].ToString();
            var report = StiReport.CreateNewDashboard();
            report.Load(StiNetCoreHelper.MapPath(this, $"Dashboards/{fileName}.mrt"));

            var dashboard = report.Pages[0] as StiDashboard;
            ViewBag.ForeHtmlColor = ColorTranslator.ToHtml(dashboard != null ? StiDashboardStyleHelper.GetForeColor(dashboard) : Color.Black);
            ViewBag.BackHtmlColor = ColorTranslator.ToHtml(dashboard != null ? StiDashboardStyleHelper.GetDashboardBackColor(dashboard, true) : Color.White);
            ViewBag.BackColor = dashboard != null ? StiDashboardStyleHelper.GetDashboardBackColor(dashboard, true) : Color.White;

            return View();
        }

        public IActionResult GetReport(string id)
        {
            var report = StiReport.CreateNewDashboard();





            report.Load(StiNetCoreHelper.MapPath(this, "/Dashboards/" + id + ".mrt"));

            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        public IActionResult Design(string id)
        {
            return RedirectToAction("Dashboards", "Design", new { id });
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