using Microsoft.AspNet.Identity;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxManualsController<TDbContext>: SxMaterialsController<SxManual, SxVMMaterial, TDbContext> where TDbContext: SxDbContext
    {
        public SxManualsController() :base(Enums.ModelCoreType.Manual)
        {
            if (Repo == null)
                Repo = new SxRepoManual<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpPost]
        public virtual PartialViewResult Index(SxVMManual filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel };

            var viewModel = Repo.Read(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMManual model)
        {
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<SxVMManual, SxManual>(model);

                SxManual newModel = null;

                if (isNew)
                {
                    var titleUrl = Url.SeoFriendlyUrl(model.Title);
                    redactModel.UserId = User.Identity.GetUserId();
                    redactModel.TitleUrl = titleUrl;
                    newModel = Repo.Create(redactModel);
                }
                else
                {
                    newModel = Repo.Update(redactModel, true, "Title", "Html", "Foreword", "CategoryId");
                }

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}
