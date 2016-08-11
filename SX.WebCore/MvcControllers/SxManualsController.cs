using Microsoft.AspNet.Identity;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxManualsController<TDbContext>: SxBaseController<TDbContext> where TDbContext: SxDbContext
    {
        private static SxRepoManual<TDbContext> _repo;
        public SxManualsController()
        {
            if (_repo == null)
                _repo = new SxRepoManual<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = defaultOrder };

            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<SxManual, SxVMManual>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMManual filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<SxManual, SxVMManual>(x)).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.Manual) : new SxManual { ModelCoreType = Enums.ModelCoreType.Manual };
            var viewModel = Mapper.Map<SxManual, SxVMEditManual>(model);
            ViewBag.ModelCoreType = model.ModelCoreType;
            if (model.Category != null)
                ViewBag.MaterialCategoryTitle = model.Category.Title;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditManual model)
        {
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<SxVMEditManual, SxManual>(model);

                SxManual newModel = null;

                if (isNew)
                {
                    var titleUrl = Url.SeoFriendlyUrl(model.Title);
                    redactModel.UserId = User.Identity.GetUserId();
                    redactModel.TitleUrl = titleUrl;
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "Title", "Html", "Foreword", "CategoryId");
                }

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxManual model)
        {
            if (await _repo.GetByKeyAsync(model.Id, model.ModelCoreType) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }
    }
}
