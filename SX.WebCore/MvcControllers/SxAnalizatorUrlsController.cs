using SX.WebCore.Repositories;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxAnalizatorUrlsController<TDbContext> : SxBaseController<TDbContext>
        where TDbContext : SxDbContext
    {
        private static SxRepoAnalizatorUrl<TDbContext> _repo = new SxRepoAnalizatorUrl<TDbContext>();
        public static SxRepoAnalizatorUrl<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }
    }
}
