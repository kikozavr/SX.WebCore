using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxRequestsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private SxRepoRequest<TDbContext> _repo;
        private static readonly int _pageSize = 40;
        public SxRequestsController()
        {
            if (_repo == null)
                _repo = new SxRepoRequest<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query<SxVMRequest>(filter);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMRequest filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query<SxVMRequest>(filter);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
    }
}
