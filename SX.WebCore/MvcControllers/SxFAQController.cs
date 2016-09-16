using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [AllowAnonymous]
    public abstract class SxFAQController : SxBaseController
    {
        private static SxRepoManual _repo=new SxRepoManual();
        public static SxRepoManual Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public virtual ActionResult Index(string curCat = null)
        {
            ViewBag.CategoryId = curCat;

            var viewModel = _repo.GetManualsByCategoryId(curCat)
                .Select(x => Mapper.Map<SxManual, SxVMFAQ>(x))
                .ToArray();

            return View(curCat == null ? new SxVMFAQ[0] : viewModel);
        }

        [HttpGet]
        public virtual ActionResult OldIndex()
        {
            return View();
        }
    }
}
