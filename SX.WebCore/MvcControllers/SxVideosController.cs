﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "video-redactor")]
    public abstract class SxVideosController<TDbContext> : SxBaseController<TDbContext> where TDbContext: SxDbContext
    {
        private static SxRepoVideo<TDbContext> _repo;
        public SxVideosController()
        {
            if(_repo==null)
                _repo = new SxRepoVideo<TDbContext>();
        }

        private static int _pageSize = 20;
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter).Select(x=>Mapper.Map<SxVideo, SxVMVideo>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(SxVMVideo filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<SxVideo, SxVMVideo>(x)).ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var isNew = !id.HasValue;
            var model = isNew ? new SxVideo() : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxVideo, SxVMEditVideo>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditVideo model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditVideo, SxVideo>(model);
                SxVideo newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "VideoId", "SourceUrl");

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditVideo model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("Index");
        }
    }
}
