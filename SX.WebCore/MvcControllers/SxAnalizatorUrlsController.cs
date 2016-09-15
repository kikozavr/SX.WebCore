using SX.WebCore.Repositories;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxAnalizatorUrlsController : SxBaseController
    {
        private static SxRepoAnalizatorUrl _repo = new SxRepoAnalizatorUrl();
        public static SxRepoAnalizatorUrl Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }
    }
}
