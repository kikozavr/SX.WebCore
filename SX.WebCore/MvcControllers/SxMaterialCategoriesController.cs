using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxMaterialCategoriesController<TViewModel, TDbContext> : SxBaseController<TDbContext>
        where TDbContext: SxDbContext
        where TViewModel : class, IHierarchy<TViewModel>
    {
        private static SxRepoMaterialCategory<TDbContext> _repo=new SxRepoMaterialCategory<TDbContext>();
        public static SxRepoMaterialCategory<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ActionResult Index(ModelCoreType? mct, int page=1)
        {
            if (!mct.HasValue)
                return new HttpNotFoundResult();

            var filter = new SxFilter { ModelCoreType = (ModelCoreType)mct };
            var data = _repo.Read(filter).Select(x=>Mapper.Map<SxVMMaterialCategory, TViewModel>(x)).ToArray();
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
            ViewBag.PageTitle = getPageTitle((ModelCoreType)mct);

            return View(parents);
        }
        protected static void updateTreeNodeLevel(string id, TViewModel[] all, int level)
        {
            all.Single(x => x.Id == id).Level = level;
        }
        protected static void fillMaterialCategory(TViewModel pg, TViewModel parent, TViewModel[] all)
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
        protected static string getPageTitle(ModelCoreType mct)
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

        [HttpPost]
        public virtual PartialViewResult Index(ModelCoreType mct, SxVMVideo filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, ModelCoreType= mct };

            var viewModel = _repo.Read(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.ModelCoreType = mct;
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(ModelCoreType mct, string pcid = null, string id = null)
        {
            var data = string.IsNullOrEmpty(id) ? new SxMaterialCategory { ModelCoreType = mct, ParentCategoryId = pcid } : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxMaterialCategory, SxVMMaterialCategory>(data);

            if (data.FrontPictureId.HasValue)
                ViewData["FrontPictureIdCaption"]=data.FrontPicture.Caption;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMMaterialCategory model)
        {
            var oldId = Request.Form["OldId"];

            var isNew = string.IsNullOrEmpty(model.Id) && string.IsNullOrEmpty(oldId);
            if (isNew || (!isNew && string.IsNullOrEmpty(model.Id)))
                model.Id = Url.SeoFriendlyUrl(model.Title);

            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMMaterialCategory, SxMaterialCategory>(model);
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, oldId: oldId);

                return RedirectToAction("Index", new { mct = model.ModelCoreType });
            }
            else
            {
                return View(model);
            }
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
        public virtual async Task<PartialViewResult> FindTreeView(ModelCoreType mct)
        {
            var filter = new SxFilter { ModelCoreType = mct };

            var data = (await _repo.ReadAsync(filter)).Select(x=>Mapper.Map<SxVMMaterialCategory, TViewModel>(x)).ToArray();

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

        [HttpGet]
        public virtual PartialViewResult TreeViewMenu(ModelCoreType mct, string cur = null)
        {
            ViewBag.CurrentCategory = cur;

            var filter = new SxFilter { ModelCoreType = mct };
            var data = _repo.Read(filter).Select(x => Mapper.Map<SxVMMaterialCategory, TViewModel>(x)).ToArray(); ;
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
            ViewBag.TreeViewMenuFuncContent = TreeViewMenuFuncContent(mct);

            return PartialView("_TreeViewMenu", parents);
        }

        private Func<TViewModel, string> TreeViewMenuFuncContent(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Aphorism:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "Aphorisms", new { curCat = x.Id }), x.Title);
                case ModelCoreType.Manual:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "FAQ", new { curCat = x.Id }), x.Title);
                default:
                    return null;
            }
        }
    }
}
