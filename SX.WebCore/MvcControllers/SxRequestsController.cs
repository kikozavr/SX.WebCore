using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxRequestsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static readonly int _pageSize = 40;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var repo = new SxRepoRequest<TDbContext>();
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = repo.Count(filter);
            var data = repo.Query(filter);
            var viewModel=data
                .Select(x=>Mapper.Map<SxRequest, SxVMRequest>(x))
                .ToArray();
            
            ViewBag.Filter = filter;
            
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMRequest filterModel, SxOrder order, int page = 1)
        {
            var repo = new SxRepoRequest<TDbContext>();
            var filter = new SxFilter(page, _pageSize) { Order = order!=null && order.Direction!=SortDirection.Unknown? order:null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = repo.Count(filter);
            var data = repo.Query(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxRequest, SxVMRequest>(x))
                .ToArray();
            
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
    }
}
