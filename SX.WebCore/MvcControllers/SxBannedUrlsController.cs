﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxBannedUrlsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoBannedUrl<TDbContext> _repo;
        public SxBannedUrlsController()
        {
            if(_repo==null)
                _repo = new SxRepoBannedUrl<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dbu.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query<SxVMBannedUrl>(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMBannedUrl filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query<SxVMBannedUrl>(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxBannedUrl();
            var seoInfo = Mapper.Map<SxBannedUrl, SxVMEditBannedUrl>(model);
            return View(seoInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditBannedUrl model)
        {
            if (_repo.All.SingleOrDefault(x => x.Url == model.Url) != null)
                ModelState.AddModelError("Url", "Такая запись уже содержится в БД");

            var redactModel = Mapper.Map<SxVMEditBannedUrl, SxBannedUrl>(model);

            if (ModelState.IsValid)
            {
                SxBannedUrl newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Url", "Couse");

                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditBannedUrl model)
        {
            if (_repo.GetByKey(model.Id) != null)
                _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}