using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestBlocksController<TDbContext> :SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTestBlock<TDbContext> _repo;
        public SxSiteTestBlocksController()
        {
            if(_repo==null)
                _repo = new SxRepoSiteTestBlock<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dstb.Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);

            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestBlock, SxVMSiteTestBlock>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMSiteTestBlock filterModel, SxOrder order, int page = 1)
        {
            var testTitle = Request.Form["filterModel[TestTitle]"];
            if (!string.IsNullOrEmpty(testTitle))
                filterModel.Test = new SxVMSiteTest { Title = testTitle.ToString() };

            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestBlock, SxVMSiteTestBlock>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(int testId, SxVMSiteTestBlock filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            if (filterModel == null && testId != 0)
                filterModel = new SxVMSiteTestBlock();
            filterModel.TestId = testId;
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= pageSize ? 1 : page;
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestBlock, SxVMSiteTestBlock>(x))
                .ToArray();

            ViewBag.Filter = filter;
            ViewBag.SiteTestId = testId;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestBlock();
            if (id.HasValue)
                ViewBag.SiteTestTitle = new SxRepoSiteTest<TDbContext>().GetByKey(model.TestId).Title;
            return View(Mapper.Map<SxSiteTestBlock, SxVMEditSiteTestBlock>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditSiteTestBlock model)
        {
            if (model.TestId == 0)
                ModelState.AddModelError("TestId", "Выберите тест");

            var redactModel = Mapper.Map<SxVMEditSiteTestBlock, SxSiteTestBlock>(model);
            if (ModelState.IsValid)
            {
                SxSiteTestBlock newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "TestId", "Description");
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(SxVMEditSiteTestBlock model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}
