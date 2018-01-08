using System.Web.Mvc;
using toofz.NecroDancer.Web.Api.Areas.HelpPage.Controllers;

namespace toofz.NecroDancer.Web.Api.Controllers
{
    public sealed class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(HelpController.Index), "Help");
        }
    }
}