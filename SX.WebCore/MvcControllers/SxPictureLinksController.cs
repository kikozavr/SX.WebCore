using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "photo-redactor")]
    public abstract class SxPictureLinksController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoPicture<TDbContext> _repo;
        public SxPictureLinksController()
        {
            if (_repo == null)
                _repo = new SxRepoPicture<TDbContext>();
        }

        private static readonly int _pageSize = 20;
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dp.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order= defaultOrder, AddintionalInfo= new object[] { mid, mct} };
            filter.PagerInfo.TotalItems = _repo.LinkedPicturesCount(filter, fm);
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedPictures(filter, fm);
            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, SxVMPicture filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dp.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order==null || order.Direction==SortDirection.Unknown? defaultOrder:order, WhereExpressionObject = filterModel, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedPicturesCount(filter, fm);
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedPictures(filter, fm);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, 10) { Order=defaultOrder, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedPicturesCount(filter, fm);
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedPictures(filter, fm);
            return PartialView("_GridViewNotLinked", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, SxVMPicture filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, 10) { Order = order==null || order.Direction==SortDirection.Unknown?defaultOrder:order, WhereExpressionObject = filterModel, AddintionalInfo=new object[] { mid, mct } };
            filter.PagerInfo.TotalItems = _repo.LinkedPicturesCount(filter, fm); ;
            ViewBag.Filter = filter;
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedPictures(filter, fm);
            return PartialView("_GridViewNotLinked", viewModel);
        }

        [HttpPost]
        public virtual RedirectToRouteResult AddMaterialPicture(int mid, ModelCoreType mct)
        {
            var pictures = Request.Form.GetValues("picture");
            if (pictures != null && pictures.Any())
            {
                for (int i = 0; i < pictures.Length; i++)
                {
                    var pictureId = Guid.Parse(pictures[i]);
                    _repo.AddMaterialPicture(mid, mct, pictureId);
                }
            }
            return RedirectToAction("Index", "PictureLinks", new { mid= mid, mct= mct, fm=true, page=1 });
        }

        [HttpPost]
        public virtual RedirectToRouteResult DeleteMaterialPicture(int mid, ModelCoreType mct, Guid pid)
        {
            _repo.DeleteMaterialPicture(mid, mct, pid);
            return RedirectToAction("Index", "PictureLinks", new { mid = mid, mct = mct, fm = true });
        }
    }
}
