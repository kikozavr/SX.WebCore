using SX.WebCore.Repositories;

namespace SX.WebCore.MvcControllers
{
    internal sealed class SxAnalizatorUrlsController<TDbContext> : SxBaseController<TDbContext>
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
