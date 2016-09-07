using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public class SxBannerGroupsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoBannerGroup<TDbContext> _repo=new SxRepoBannerGroup<TDbContext>();
        public static SxRepoBannerGroup<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async virtual Task<PartialViewResult> Index(SxVMBannerGroup filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = (await _repo.ReadAsync(filter));

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBannerGroup();
            var viewModel = Mapper.Map<SxBannerGroup, SxVMBannerGroup>(model);
            if (id.HasValue)
                ViewBag.BannerGroupId = model.Id;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMBannerGroup model)
        {
            
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMBannerGroup, SxBannerGroup>(model);
                SxBannerGroup newModel = null;
                if (model.Id == Guid.Empty)
                {
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "Title", "Description");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxBannerGroup model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual RedirectToRouteResult AddBanner(Guid bgid, Guid bid)
        {
            _repo.AddBanner(bgid, bid);
            return RedirectToAction("Edit", new { id = bgid });
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> DeleteBanner(Guid bgid, Guid bid)
        {
            _repo.DeleteBanner(bgid, bid);

            var filter = new SxFilter(1, 20) { WhereExpressionObject = new SxVMBanner { BannerGroupId = bgid }, AddintionalInfo=new object[] { true } };

            var viewModel = await SxBannersController<TDbContext>.Repo.ReadAsync(filter);
            
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            
            return PartialView("_GroupBanners", viewModel);
        }
    }
}
