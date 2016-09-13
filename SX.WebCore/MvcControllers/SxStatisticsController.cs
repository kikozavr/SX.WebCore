using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public class SxStatisticsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageUserLoginsSize = 20;
        private static SxRepoStatistic<TDbContext> _repo=new SxRepoStatistic<TDbContext>();
        public static SxRepoStatistic<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public virtual ActionResult UserLogins(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order };
            
            var viewModel = _repo.UserLogins(filter)
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View("UserLogins", viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> UserLogins(SxVMStatisticUserLogin filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageUserLoginsSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var data = await _repo.UserLoginsAsync(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxStatisticUserLogin, SxVMStatisticUserLogin>(x))
                .ToArray();

            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_UserLoginsGridView", viewModel);
        }
    }
}
