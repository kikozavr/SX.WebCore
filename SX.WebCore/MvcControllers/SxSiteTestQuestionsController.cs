using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestQuestionsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTestQuestion<TDbContext> _repo;
        public SxSiteTestQuestionsController()
        {
            if(_repo==null)
                _repo = new SxRepoSiteTestQuestion<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMSiteTestQuestion filterModel, SxOrder order, int page = 1)
        {
            var blockTitle = Request.Form["filterModel[BlockTitle]"];
            var testTitle = Request.Form["filterModel[TestTitle]"];
            if (!string.IsNullOrEmpty(blockTitle) || !string.IsNullOrEmpty(testTitle))
            {
                filterModel.Block = new SxVMSiteTestBlock
                {
                    Title = blockTitle,
                    Test = string.IsNullOrEmpty(testTitle) ? null : new SxVMSiteTest { Title = testTitle }
                };
            }

            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestQuestion, SxVMSiteTestQuestion>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestQuestion();
            if (id.HasValue)
            {
                ViewBag.SiteTestName = model.Block.Test.Title;
                ViewBag.SiteTestBlockName = model.Block.Title;
            }
            return View(Mapper.Map<SxSiteTestQuestion, SxVMEditSiteTestQuestion>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditSiteTestQuestion model)
        {
            if (model.BlockId == 0)
                ModelState.AddModelError("BlockId", "Выберите блок теста");

            var redactModel = Mapper.Map<SxVMEditSiteTestQuestion, SxSiteTestQuestion>(model);
            if (ModelState.IsValid)
            {
                SxSiteTestQuestion newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Text", "BlockId", "IsCorrect");
                return RedirectToAction("index");
            }
            else
            {
                if (model.Id != 0)
                {
                    var old = _repo.GetByKey(model.Id);
                    ViewBag.SiteTestName = old.Block.Test.Title;
                    ViewBag.SiteTestBlockName = old.Block.Title;
                }
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(SxVMEditSiteTestQuestion model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}
