using Newtonsoft.Json;
using SX.WebCore.Attrubutes;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            
            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            var showsCount = 0;
            var clicksCount = 0;
            _repo.GetStatistic(out showsCount, out clicksCount);
            ViewBag.ShowsCount = showsCount;
            ViewBag.ClicksCount = clicksCount;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> Index(SxVMBanner filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var viewModel =await _repo.ReadAsync(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBanner();
            var viewModel = Mapper.Map<SxBanner, SxVMBanner>(model);
            viewModel.Place = viewModel.Place == SxBanner.BannerPlace.Unknown ? null : viewModel.Place;
            if (!id.HasValue)
                viewModel.PictureId = null;
            else
                ViewData["PictureIdCaption"] = model.Picture.Caption;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMBanner model)
        {
            var redactModel = Mapper.Map<SxVMBanner, SxBanner>(model);

            if (ModelState.IsValid)
            {
                SxBanner newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "PictureId", "Url", "Place", "RawUrl", "Description");

                return RedirectToAction("Index", "Banners");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxBanner model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> FindGridView(Guid bgid, SxVMBanner filterModel = null, int page = 1, int pageSize = 10)
        {
            filterModel.BannerGroupId = bgid;
            ViewBag.Filter = filterModel;
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel, AddintionalInfo=new object[] { false } };

            var viewModel =await _repo.ReadAsync(filter);

            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult GroupBanners(Guid bgid, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { AddintionalInfo=new object[] { true, false, bgid } };
            
            var viewModel = _repo.Read(filter);

            ViewBag.BannerGroupId = bgid;
            ViewBag.Filter = filter;

            return PartialView("_GroupBanners", viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> GroupBanners(Guid bgid, SxVMBanner filterModel, SxOrder order, int page = 1, int pageSize = 10, bool forGroup=true)
        {
            filterModel.BannerGroupId = bgid;
            var filter = new SxFilter(page, pageSize) { Order = order, AddintionalInfo=new object[] { forGroup, false, bgid } };
            
            var viewModel =await _repo.ReadAsync(filter);

            ViewBag.BannerGroupId = bgid;
            ViewBag.ForGroup = forGroup;
            ViewBag.Filter = filter;

            return PartialView("_GroupBanners", viewModel);
        }

        [HttpGet, AllowAnonymous]
        public virtual async Task<ActionResult> Click(Guid bannerId)
        {
            var banner = await _repo.GetByKeyAsync(bannerId);

            if (banner == null)
                return new HttpNotFoundResult();
            else
            {
                var affiliateCookieName = ConfigurationManager.AppSettings["AffiliateCookieName"];
                var cookies = Request.Cookies[affiliateCookieName];
                if (cookies != null)
                    await _repo.AddClickAsync(bannerId, JsonConvert.DeserializeObject<List<string>>(cookies.Value));
                else
                    await _repo.AddClickAsync(bannerId);
                return Redirect(banner.Url);
            }
        }

        [HttpPost, AllowAnonymous, NotLogRequest]
        public async virtual Task<JsonResult> AddShow(Guid bannerId)
        {
            await _repo.AddShowsAsync(new Guid[] { bannerId });
            return Json(new { Success = true });
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
