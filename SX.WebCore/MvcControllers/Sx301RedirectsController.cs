﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class Sx301RedirectsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepo301Redirect<TDbContext> _repo;
        public Sx301RedirectsController()
        {
            if(_repo==null)
                _repo = new SxRepo301Redirect<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query<SxVM301Redirect>(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVM301Redirect filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query<SxVM301Redirect>(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new Sx301Redirect();
            var viewModel = Mapper.Map<Sx301Redirect, SxVMEdit301Redirect>(model);
            return View(viewModel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEdit301Redirect model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEdit301Redirect, Sx301Redirect>(model);
                Sx301Redirect newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "OldUrl", "NewUrl");
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEdit301Redirect model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}
