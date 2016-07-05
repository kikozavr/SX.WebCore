﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteTestSubjectsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteTestSubject<TDbContext> _repo;
        public SxSiteTestSubjectsController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteTestSubject<TDbContext>();
        }

        private static int _pageSize = 10;

        [HttpPost]
        public virtual PartialViewResult Index(int testId, SxVMSiteTestSubject filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dstq.Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { testId } };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestSubject, SxVMSiteTestSubject>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int testId, int? id)
        {
            ViewBag.Prefix = "subject";
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestSubject() { TestId = testId };
            if (id.HasValue)
            {
                if (model == null)
                    return new HttpNotFoundResult();

                if (model.Picture != null)
                    ViewData["PictureIdCaption"] = model.Picture.Caption;
            }
            return PartialView("_Edit", Mapper.Map<SxSiteTestSubject, SxVMEditSiteTestSubject>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Prefix ="subject")] SxVMEditSiteTestSubject model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditSiteTestSubject, SxSiteTestSubject>(model);
                SxSiteTestSubject newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "TestId", "Title", "Description", "PictureId");

                return getResult(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult Delete(SxVMEditSiteTestSubject model)
        {
            _repo.Delete(model.Id);
            return getResult(model);
        }

        private PartialViewResult getResult(SxVMEditSiteTestSubject model)
        {
            var defaultOrder = new SxOrder { FieldName = "dstq.Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(1, _pageSize) { Order = defaultOrder, AddintionalInfo = new object[] { model.TestId } };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSiteTestSubject, SxVMSiteTestSubject>(x))
                .ToArray();
            ViewBag.Filter = filter;
            return PartialView("_GridView", viewModel);
        }
    }
}