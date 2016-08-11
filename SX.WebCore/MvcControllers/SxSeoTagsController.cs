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

            var viewModel = _repo.Read(filter)
                .Select(x=>Mapper.Map<SxSeoTags, SxVMSeoTags>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMSeoTags filterModel, SxOrder order, int page = 1)
        {
            filterModel.Keywords = null;
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter)
                .Select(x => Mapper.Map<SxSeoTags, SxVMSeoTags>(x))
                .ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

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
            var seoTags = Mapper.Map<SxSeoTags, SxVMEditSeoTags>(model);
            if (id.HasValue)
                seoTags.Keywords = model.Keywords.Select(x => Mapper.Map<SxSeoKeyword, SxVMSeoKeyword>(x)).ToArray();
            return PartialView("_EditForMaterial", seoTags);
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
                    _repo.UpdateMaterialSeoTags((int)model.MaterialId, (Enums.ModelCoreType)model.ModelCoreType, newModel.Id);
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
        public virtual async Task<PartialViewResult> DeleteForMaterial(SxSeoTags model)
        {
            await _repo.UpdateMaterialSeoTagsAsync((int)model.MaterialId, (ModelCoreType)model.ModelCoreType, null);

            await _repo.DeleteAsync(model);

            TempData["ModelSeoInfoRedactInfo"] = "Успешно удалено";
            return PartialView("_EditForMaterial", new SxVMEditSeoTags { MaterialId = model.MaterialId, ModelCoreType = model.ModelCoreType, Id = 0 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxSeoTags model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }
    }
}
