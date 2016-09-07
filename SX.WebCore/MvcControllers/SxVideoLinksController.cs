using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "video-redactor")]
    public class SxVideoLinksController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoVideo<TDbContext> _repo=new SxRepoVideo<TDbContext>();
        public static SxRepoVideo<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static readonly int _pageSize = 20;
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order= defaultOrder, AddintionalInfo= new object[] { mid, mct} };
            filter.PagerInfo.TotalItems = _repo.LinkedVideosCount(filter, fm);
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, SxVMVideo filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order==null || order.Direction==SortDirection.Unknown? defaultOrder:order, WhereExpressionObject = filterModel, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedVideosCount(filter, fm);
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, 10) { Order=defaultOrder, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedVideosCount(filter, fm);
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("_GridViewNotLinked", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, SxVMVideo filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, 10) { Order = order==null || order.Direction==SortDirection.Unknown?defaultOrder:order, WhereExpressionObject = filterModel, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedVideosCount(filter, fm); ;
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("_GridViewNotLinked", viewModel);
        }

        [HttpPost]
        public virtual RedirectToRouteResult AddMaterialVideo(int mid, ModelCoreType mct)
        {
            var videos = Request.Form.GetValues("video");
            if (videos != null && videos.Any())
            {
                for (int i = 0; i < videos.Length; i++)
                {
                    var videoId = Guid.Parse(videos[i]);
                    _repo.AddMaterialVideo(mid, mct, videoId);
                }
            }
            return RedirectToAction("index", new { mid= mid, mct= mct, fm=true, page=1 });
        }

        [HttpPost]
        public virtual RedirectToRouteResult DeleteMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            _repo.DeleteMaterialVideo(mid, mct, vid);
            return RedirectToAction("index", new { mid = mid, mct = mct, fm = true });
        }
    }
}
