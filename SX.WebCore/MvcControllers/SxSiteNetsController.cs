﻿using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "smm")]
    public class SxSiteNetsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSiteNet<TDbContext> _repo=new SxRepoSiteNet<TDbContext>();
        public static SxRepoSiteNet<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dn.Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            
            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> Index(SxVMSiteNet filterModel, SxOrder order, int page = 1)
        {
            var netName = Request.Form.Get("filterModel[NetName]");
            if (netName != null)
                filterModel.Net = new SxVMNet { Name = netName };

            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var viewModel =await _repo.ReadAsync(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteNet();
            if (id.HasValue && model == null)
                return new HttpNotFoundResult();
            var viewModel = Mapper.Map<SxSiteNet, SxVMSiteNet>(model);
            return View(viewModel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMSiteNet model)
        {
            var isNew = model.NetId == 0;
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMSiteNet, SxSiteNet>(model);
                SxSiteNet newModel = null;
                newModel = _repo.Update(redactModel);
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxSiteNet model)
        {
            var data = _repo.GetByKey(model.NetId);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index");
        }
    }
}