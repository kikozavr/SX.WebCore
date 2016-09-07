using SX.WebCore.Repositories;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize]
    public abstract class SxChatController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepositoryChat<TDbContext> _repo=new SxRepositoryChat<TDbContext>();
        public static SxRepositoryChat<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OnlineUsers()
        {
            if (!Request.IsAjaxRequest())
                return new HttpNotFoundResult();

            var data = _repo.OnlineUsers;
            return PartialView("_OnlineUsers", data);
        }
    }
}
