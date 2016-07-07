using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxNetsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoNet<TDbContext> _repo;
        public SxNetsController()
        {
            if(_repo==null)
                _repo = new SxRepoNet<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dn.Name", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter).Select(x=>Mapper.Map<SxNet, SxVMNet>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMNet filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<SxNet, SxVMNet>(x)).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
    }
}
