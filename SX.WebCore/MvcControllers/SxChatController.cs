using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize]
    public abstract class SxChatController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}
