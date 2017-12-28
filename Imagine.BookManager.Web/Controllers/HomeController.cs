using System.Web.Mvc;

namespace Imagine.BookManager.Web.Controllers
{
    public class HomeController : BookManagerControllerBase
    {
        public ActionResult Index()
        {

            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }

    }
}