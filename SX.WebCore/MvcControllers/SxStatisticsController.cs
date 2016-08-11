using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxStatisticsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageUserLoginsSize = 20;
        private SxRepoStatistic<TDbContext> _repo;
        public SxStatisticsController()
        {
            _repo = new SxRepoStatistic<TDbContext>();
        }

        [HttpGet]
        public virtual ActionResult UserLogins(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order };
            
            var viewModel = _repo.UserLogins(filter)
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View("UserLogins", viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> UserLogins(SxVMStatisticUserLogin filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var data = await _repo.UserLoginsAsync(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageUserLoginsSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_UserLoginsGridView", viewModel);
        }
    }
}
