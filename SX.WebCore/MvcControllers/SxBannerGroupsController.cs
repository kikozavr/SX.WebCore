using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxBannerGroupsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoBannerGroup<TDbContext> _repo;
        public SxBannerGroupsController()
        {
            if(_repo==null)
                _repo = new SxRepoBannerGroup<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter)
                .Select(x=>Mapper.Map<SxBannerGroup, SxVMBannerGroup>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMBannerGroup filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter)
                .Select(x => Mapper.Map<SxBannerGroup, SxVMBannerGroup>(x))
                .ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBannerGroup();
            var viewModel = Mapper.Map<SxBannerGroup, SxVMEditBannerGroup>(model);
            if (id.HasValue)
                ViewBag.BannerGroupId = model.Id;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditBannerGroup model)
        {
            
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditBannerGroup, SxBannerGroup>(model);
                SxBannerGroup newModel = null;
                if (model.Id == Guid.Empty)
                {
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "Title");
                }

                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditBannerGroup model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public virtual RedirectToRouteResult AddBanner(Guid bgid, Guid bid)
        {
            _repo.AddBanner(bgid, bid);
            return RedirectToAction("edit", new { id = bgid });
        }

        [HttpPost]
        public virtual PartialViewResult DeleteBanner(Guid bgid, Guid bid)
        {
            _repo.DeleteBanner(bgid, bid);

            var repoBanner = new SxRepoBanner<TDbContext>();
            var filter = new SxFilter(1, 20) { WhereExpressionObject = new SxVMBanner { BannerGroupId = bgid } };
            var totalItems = repoBanner.Count(filter, true);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = repoBanner.Query(filter, true);
            return PartialView("_GroupBanners", viewModel);
        }
    }
}
