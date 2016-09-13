using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public class SxRedirectsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepo301Redirect<TDbContext> _repo=new SxRepo301Redirect<TDbContext>();
        public static SxRepo301Redirect<TDbContext> Repo
        {
            get
            {
                return _repo;
            }
            set
            {
                _repo = value;
            }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            
            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(SxVM301Redirect filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var viewModel =await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new Sx301Redirect();
            var viewModel = Mapper.Map<Sx301Redirect, SxVM301Redirect>(model);
            return View(viewModel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVM301Redirect model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVM301Redirect, Sx301Redirect>(model);
                Sx301Redirect newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "OldUrl", "NewUrl");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(Sx301Redirect model)
        {
            var data = _repo.GetByKey(model.Id);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index");
        }
    }
}
