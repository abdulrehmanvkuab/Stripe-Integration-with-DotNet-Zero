using Abp.Organizations;
using Arch.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Arch.Web.Controllers
{
    [Controller]
    public class DesignController : Controller
    {
        //static DesignController()
        //{
        //    // How to Activate
        //    //Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnO...";
        //    //Stimulsoft.Base.StiLicense.LoadFromFile("license.key");
        //    //Stimulsoft.Base.StiLicense.LoadFromStream(stream);
        //}
        private IWebHostEnvironment environment;
        private string rptname;
        private string connectionString;

        readonly IConfigurationRoot _appConfiguration;
        protected OrganizationUnitManager _OrganizationalUnitManager;
        public DesignController(IWebHostEnvironment environment, IAppConfigurationAccessor appConfigurationAccessor, OrganizationUnitManager OrganizationalUnitManager)
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

            this.environment = environment;
        }
        public IActionResult Dashboards()
        {
            return View();
        }

        public IActionResult GetReport(string id)
        {
            var report = StiReport.CreateNewDashboard();
            report.Load(StiNetCoreHelper.MapPath(this, "Reports/Dashboards/" + id + ".mrt"));

            return StiNetCoreDesigner.GetReportResult(this, report);
        }

        public IActionResult SaveReport()
        {
            var report = StiNetCoreDesigner.GetReportObject(this);

            // string packedReport = report.SavePackedReportToString();
            // ...
            // The save report code here
            // ...

            // Completion of the report saving without dialog box
            return StiNetCoreDesigner.SaveReportResult(this);
        }

        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

        public IActionResult ExitDesigner(string id)
        {
            return RedirectToAction("Dashboards", "View", new { id });
        }
    }
}