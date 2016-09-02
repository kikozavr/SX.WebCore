﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
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

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<PartialViewResult> Index(SxVMAffiliateLink filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new SxAffiliateLink();
            if (data == null)
                return new HttpNotFoundResult();

            var viewModel = Mapper.Map<SxAffiliateLink, SxVMAffiliateLink>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(SxVMAffiliateLink model)
        {
            var isNew = model.Id == Guid.Empty;

            if(ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMAffiliateLink, SxAffiliateLink>(model);
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel, true, "Description", "ClickCost");

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
