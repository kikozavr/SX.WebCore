using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    public class SxErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
