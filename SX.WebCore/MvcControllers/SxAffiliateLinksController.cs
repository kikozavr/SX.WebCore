using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxAffiliateLinksController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoAffiliateLink<TDbContext> _repo;
        public SxAffiliateLinksController()
        {
            if (_repo == null)
                _repo = new SxRepoAffiliateLink<TDbContext>();
        }

        private static readonly int _pageSize = 20;

        [HttpGet]
        public ActionResult Index(int page=1)
        {
            var order = new SxOrder { FieldName = "dal.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            var totalItems = 0;
            var data = _repo.Read(filter, out totalItems);
            filter.PagerInfo.TotalItems = totalItems;
            var viewModel = data
                .Select(x => Mapper.Map<SxAffiliateLink, SxVMAffiliateLink>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(SxVMAffiliateLink filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var totalItems = 0;
            var data = _repo.Read(filter, out totalItems);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            var viewModel = data
                .Select(x => Mapper.Map<SxAffiliateLink, SxVMAffiliateLink>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new SxAffiliateLink();
            if (data == null)
                return new HttpNotFoundResult();

            var viewModel = Mapper.Map<SxAffiliateLink, SxVMEditAffiliateLink>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(SxVMEditAffiliateLink model)
        {
            var isNew = model.Id == Guid.Empty;

            if(ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditAffiliateLink, SxAffiliateLink>(model);
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(SxAffiliateLink model)
        {
            var data = _repo.GetByKey(model.Id);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index");
        }
    }
}
