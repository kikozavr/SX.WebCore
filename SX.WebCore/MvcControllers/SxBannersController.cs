using SX.WebCore.Attrubutes;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxBannersController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoBanner<TDbContext> _repo;
        public SxBannersController()
        {
            if(_repo==null)
                _repo = new SxRepoBanner<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "db.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxBanner, SxVMBanner>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMBanner filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter)
                .Select(x=>Mapper.Map<SxBanner, SxVMBanner>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBanner();
            var viewModel = Mapper.Map<SxBanner, SxVMEditBanner>(model);
            viewModel.Place = viewModel.Place == SxBanner.BannerPlace.Unknown ? null : viewModel.Place;
            if (!id.HasValue)
                viewModel.PictureId = null;
            else
                ViewData["PictureIdCaption"] = model.Picture.Caption;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditBanner model)
        {
            var redactModel = Mapper.Map<SxVMEditBanner, SxBanner>(model);

            if (ModelState.IsValid)
            {
                SxBanner newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "PictureId", "Url", "Place", "ControllerName", "ActionName");

                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditBanner model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(Guid bgid, SxVMBanner filterModel = null, int page = 1, int pageSize = 10)
        {
            filterModel.BannerGroupId = bgid;
            ViewBag.Filter = filterModel;
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter, false);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = _repo.Query(filter, false)
                .Select(x => Mapper.Map<SxBanner, SxVMBanner>(x))
                .ToArray();

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult GroupBanners(Guid bgid, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = new SxVMBanner { BannerGroupId = bgid } };
            var totalItems = _repo.Count(filter, true);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.BannerGroupId = bgid;
            ViewBag.Filter = filter;

            var viewModel = _repo.Query(filter, true)
                .Select(x => Mapper.Map<SxBanner, SxVMBanner>(x))
                .ToArray();

            return PartialView("_GroupBanners", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult GroupBanners(Guid bgid, SxVMBanner filterModel, SxOrder order, int page = 1, int pageSize = 10, bool forGroup=true)
        {
            filterModel.BannerGroupId = bgid;
            var filter = new SxFilter(page, pageSize) { Order = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter, forGroup);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.BannerGroupId = bgid;
            ViewBag.ForGroup = forGroup;
            ViewBag.Filter = filter;

            var viewModel = _repo.Query(filter, forGroup)
                .Select(x => Mapper.Map<SxBanner, SxVMBanner>(x))
                .ToArray();

            return PartialView("_GroupBanners", viewModel);
        }

        [HttpPost, AllowAnonymous, NotLogRequest]
        public virtual async Task<JsonResult> AddClick(Guid bannerId)
        {
            return await Task.Run(() =>
            {
                _repo.AddClick(bannerId);
                return Json(new { Success=true});
            });
        }

        [HttpPost, AllowAnonymous, NotLogRequest]
        public async virtual Task<JsonResult> AddShow(Guid bannerId)
        {
            return await Task.Run(() =>
            {
                _repo.AddShows(new Guid[] { bannerId });
                return Json(new { Success = true });
            });
        }

        [HttpGet]
#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public async Task<JsonResult> DateStatistic()
        {
            return await Task.Run(() =>
            {
                var data = _repo.DateStatistic;
                return Json(data, JsonRequestBehavior.AllowGet);
            });
        }
    }
}
