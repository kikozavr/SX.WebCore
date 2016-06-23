using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="seo")]
    public abstract class SxSeoTagsController<TDbContext>: SxBaseController<TDbContext> where TDbContext: SxDbContext
    {
        private static SxRepoSeoTags<TDbContext> _repo;
        public SxSeoTagsController()
        {
            if(_repo==null)
                _repo = new SxRepoSeoTags<TDbContext>();
        }

        private static int _pageSize = 20;
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter)
                .Select(x=>Mapper.Map<SxSeoTags, SxVMSeoTags>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMSeoTags filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSeoTags, SxVMSeoTags>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoTags();
            var seoInfo = Mapper.Map<SxSeoTags, SxVMEditSeoTags>(model);
            if (id.HasValue)
                seoInfo.Keywords = model.Keywords.Select(x => Mapper.Map<SxSeoKeyword, SxVMSeoKeyword>(x)).ToArray();
            return View(seoInfo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditSeoTags model)
        {
            var redactModel = Mapper.Map<SxVMEditSeoTags, SxSeoTags>(model);
            redactModel.Check(ModelState);
            if (ModelState.IsValid)
            {
                SxSeoTags newModel = null;
                if (model.Id == 0)
                {
                    var existInfo = _repo.GetByRawUrl(model.RawUrl);
                    if (existInfo != null)
                    {
                        ModelState.AddModelError("RawUrl", "Информация для страницы с таким url уже содержится в БД");
                        return View(model);
                    }
                    newModel = _repo.Create(redactModel);
                }
                else
                    newModel = _repo.Update(redactModel, true, "RawUrl", "SeoTitle", "SeoDescription", "H1", "H1CssClass");

                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpGet]
        public virtual PartialViewResult EditForMaterial(int mid, ModelCoreType mct, int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoTags { MaterialId = mid, ModelCoreType = mct };
            var seoInfo = Mapper.Map<SxSeoTags, SxVMEditSeoTags>(model);
            if (id.HasValue)
                seoInfo.Keywords = model.Keywords.Select(x => Mapper.Map<SxSeoKeyword, SxVMSeoKeyword>(x)).ToArray();
            return PartialView("_EditForMaterial", seoInfo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditForMaterial(SxVMEditSeoTags model)
        {
            SxSeoTags newModel = null;
            var redactModel = Mapper.Map<SxVMEditSeoTags, SxSeoTags>(model);
            redactModel.Check(ModelState);
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;

                if (isNew)
                {
                    newModel = _repo.Create(redactModel);
                    updateMaterialSeoInfo((int)model.MaterialId, (Enums.ModelCoreType)model.ModelCoreType, newModel.Id);
                    TempData["ModelSeoInfoRedactInfo"] = "Успешно добавлено";
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "SeoTitle", "SeoDescription", "H1", "H1CssClass");
                    TempData["ModelSeoInfoRedactInfo"] = "Успешно обновлено";
                }

                var viewModel = Mapper.Map<SxSeoTags, SxVMEditSeoTags>(newModel);
                return PartialView("_EditForMaterial", viewModel);
            }
            else
                return PartialView("_EditForMaterial", model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult DeleteForMaterial(SxVMEditSeoTags model)
        {
            updateMaterialSeoInfo((int)model.MaterialId, (ModelCoreType)model.ModelCoreType, null);

            _repo.Delete(model.Id);
            TempData["ModelSeoInfoRedactInfo"] = "Успешно удалено";
            return PartialView("_EditForMaterial", new SxVMEditSeoTags { MaterialId = model.MaterialId, ModelCoreType = model.ModelCoreType, Id = 0 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditSeoTags model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }


        private static void updateMaterialSeoInfo(int mid, ModelCoreType mct, int? siid)
        {
            switch (mct)
            {
                case ModelCoreType.Article:
                    //var repoA = new RepoArticle();
                    //var art = repoA.GetByKey(mid, mct);
                    //art.SeoInfoId = siid;
                    //repoA.Update(art, true, "SeoInfoId");
                    break;
                case ModelCoreType.News:
                    //var repoN = new RepoNews();
                    //var news = repoN.GetByKey(mid, mct);
                    //news.SeoInfoId = siid;
                    //repoN.Update(news, true, "SeoInfoId");
                    break;
            }
        }
    }
}
