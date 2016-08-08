using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxMaterialCategoriesController<TDbContext> : SxBaseController<TDbContext> where TDbContext: SxDbContext
    {
        private static SxRepoMaterialCategory<TDbContext> _repo;
        public SxMaterialCategoriesController()
        {
            if(_repo==null)
                _repo = new SxRepoMaterialCategory<TDbContext>();
        }

        private static int _pageSize = 20;
        [HttpGet]
        public virtual ViewResult Index(ModelCoreType mct, int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order, ModelCoreType=mct };
            var totalItems = 0;
            var data = _repo.Read(filter, out totalItems);
            filter.PagerInfo.TotalItems = totalItems;
            var viewModel=data
                .Select(x => Mapper.Map<SxMaterialCategory, SxVMMaterialCategory>(x))
                .ToArray();

            ViewBag.ModelCoreType = mct;
            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(ModelCoreType mct, SxVMVideo filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, ModelCoreType= mct };

            var totalItems = 0;
            var data = _repo.Read(filter, out totalItems);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            var viewModel = data
                .Select(x => Mapper.Map<SxMaterialCategory, SxVMMaterialCategory>(x))
                .ToArray();

            ViewBag.ModelCoreType = mct;
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(ModelCoreType mct, string id)
        {
            var isNew = string.IsNullOrEmpty(id);
            var data = isNew ? new SxMaterialCategory { ModelCoreType=mct} : _repo.GetByKey(id);
            if (!isNew && data == null)
                return new HttpNotFoundResult();

            if (data.FrontPicture != null)
                ViewData["FrontPictureIdCaption"] = data.FrontPicture.Caption;

            ViewBag.ModelCoreType = mct;

            var viewModel = Mapper.Map<SxMaterialCategory, SxVMEditMaterialCategory>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditMaterialCategory model)
        {
            var oldId = Request.Form["OldId"];

            var isNew = string.IsNullOrEmpty(model.Id) && string.IsNullOrEmpty(oldId);
            if (isNew || (!isNew && string.IsNullOrEmpty(model.Id)))
                model.Id = Url.SeoFriendlyUrl(model.Title);

            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditMaterialCategory, SxMaterialCategory>(model);
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, oldId: oldId);

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxMaterialCategory model)
        {
            var data = _repo.GetByKey(model.Id);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index", new { mct=model.ModelCoreType});
        }

        [HttpPost]
        public virtual PartialViewResult FindTreeView(ModelCoreType mct)
        {
            var filter = new SxFilter { ModelCoreType = mct };
            var count = 0;
            var data = _repo.Read(filter, out count).Select(x=>Mapper.Map<SxMaterialCategory, SxVMMaterialCategory>(x)).ToArray();

            var parents = data.Where(x => x.ParentCategoryId == null).ToArray();
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
                parent.Level = 1;
                updateTreeNodeLevel(parent.Id, data, 1);
                fillMaterialCategory(parent, null, data);
            }

            ViewBag.MaxTreeViewLevel = data.Any() ? data.Max(x => x.Level) : 1;
            ViewBag.ModelCoreType = mct;
            ViewBag.PageTitle = getPageTitle(mct);

            return PartialView("_TreeView", parents);
        }

        private static void updateTreeNodeLevel(string id, SxVMMaterialCategory[] all, int level)
        {
            all.Single(x => x.Id == id).Level = level;
        }
        private static void fillMaterialCategory(SxVMMaterialCategory pg, SxVMMaterialCategory parent, SxVMMaterialCategory[] all)
        {
            var children = all.Where(x => x.ParentCategoryId == pg.Id).ToArray();
            if (!children.Any()) return;

            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.Level = pg.Level + 1;
                updateTreeNodeLevel(child.Id, all, child.Level);
                fillMaterialCategory(child, pg, all);
            }

            pg.ChildCategories = children.OrderBy(x => x.Title).ToArray();
        }
        private static string getPageTitle(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Article:
                    return "Категории статей";
                case ModelCoreType.News:
                    return "Категории новостей";
                case ModelCoreType.Manual:
                    return "Справочные категории";
                case ModelCoreType.Aphorism:
                    return "Категории афоризмов";
                default:
                    return "Категоря материалов не определена";
            }
        }
    }
}
