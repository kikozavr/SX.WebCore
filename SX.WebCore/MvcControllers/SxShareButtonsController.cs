using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxShareButtonsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoShareButton<TDbContext> _repo=new SxRepoShareButton<TDbContext>();
        public static SxRepoShareButton<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "NetName", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(SxVMShareButton filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpNotFoundResult();

            var model = id.HasValue ? _repo.GetByKey(id) : new SxShareButton();
            var viewModel = Mapper.Map<SxShareButton, SxVMShareButton>(model);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMShareButton model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMShareButton, SxShareButton>(model);
                SxShareButton newModel = null;
                if (model.Id == 0)
                    return new HttpNotFoundResult();
                else
                    newModel = _repo.Update(redactModel, true, "Show", "ShowCounter");
                return RedirectToAction("index");
            }
            else
                return View(model);
        }
    }
}
