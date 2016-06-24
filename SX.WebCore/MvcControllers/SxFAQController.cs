using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [AllowAnonymous]
    public abstract class SxFAQController<TDbContext>:SxBaseController<TDbContext> where TDbContext: SxDbContext
    {
        private static SxRepoManual<TDbContext> _repo;
        public SxFAQController()
        {
            if(_repo==null)
                _repo = new SxRepoManual<TDbContext>();
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
