using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="seo")]
    public class SxMaterialTagsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoMaterialTag<TDbContext> _repo=new SxRepoMaterialTag<TDbContext>();
        public static SxRepoMaterialTag<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private int _pageSize = 10;
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { MaterialId = mid, ModelCoreType = mct, Order = defaultOrder };

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> Index(int mid, ModelCoreType mct, SxVMMaterialTag filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order==null || order.Direction==SortDirection.Unknown?defaultOrder:order, WhereExpressionObject = filterModel, MaterialId = mid, ModelCoreType = mct };

            var viewModel = await _repo.ReadAsync(filter);

            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMMaterialTag model)
        {
            model.Id = UrlHelperExtensions.SeoFriendlyUrl(model.Title.Trim());
            ModelState["Id"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (_repo.GetByKey(model.Id, model.ModelCoreType) != null)
                {
                    ModelState.AddModelError("Id", "Такой тег уже добавлен для данного типа материалов");
                    return RedirectToAction("Index", "MaterialTags", new { mid = model.MaterialId, mct = model.ModelCoreType });
                }

                var redactModel = Mapper.Map<SxVMMaterialTag, SxMaterialTag>(model);
                _repo.Create(redactModel);
            }

            return RedirectToAction("Index", "MaterialTags", new { mid = model.MaterialId, mct = model.ModelCoreType });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxMaterialTag model)
        {
            var data = _repo.GetByKey(model.Id, model.ModelCoreType);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index", "MaterialTags", new { mid = model.MaterialId, mct = model.ModelCoreType });
        }
    }
}
