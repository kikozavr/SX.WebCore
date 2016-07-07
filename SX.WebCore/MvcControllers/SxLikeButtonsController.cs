using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxLikeButtonsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoLikeButton<TDbContext> _repo;
        public SxLikeButtonsController()
        {
            if(_repo==null)
                _repo = new SxRepoLikeButton<TDbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "NetName", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            var viewModel = _repo.Query(filter).Select(x=>Mapper.Map<SxLikeButton, SxVMLikeButton>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMLikeButton filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<SxLikeButton, SxVMLikeButton>(x)).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpNotFoundResult();

            var model = id.HasValue ? _repo.GetByKey(id) : new SxLikeButton();
            var viewModel = Mapper.Map<SxLikeButton, SxVMEditLikeButton>(model);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditLikeButton model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditLikeButton, SxLikeButton>(model);
                SxLikeButton newModel = null;
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
