using Microsoft.AspNetCore.Mvc;

namespace Arch.Web.Controllers
{
    public class DesignerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
