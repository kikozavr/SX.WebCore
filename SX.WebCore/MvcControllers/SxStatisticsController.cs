using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxStatisticsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageUserLoginsSize = 20;

        [HttpGet]
        public virtual ActionResult UserLogins(int page = 1)
        {
            var repo = new SxRepoStatistic<TDbContext>();
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order };
            filter.PagerInfo.TotalItems = repo.UserLoginsCount(filter);
            var data = repo.UserLogins(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View("UserLogins", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult UserLogins(SxVMStatisticUserLogin filterModel, SxOrder order, int page = 1)
        {
            var repo = new SxRepoStatistic<TDbContext>();
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = repo.UserLoginsCount(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageUserLoginsSize ? 1 : page;
            var data = repo.UserLogins(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_UserLoginsGridView", viewModel);
        }
    }
}
