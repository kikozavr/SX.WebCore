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
    public abstract class SxRedirectsController : SxBaseController
    {
        private static SxRepoRedirect _repo =new SxRepoRedirect();
        public static SxRepoRedirect Repo
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
        public virtual async Task<ActionResult> Index(SxVMRedirect filterModel, SxOrder order, int page = 1)
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
            var model = id.HasValue ? _repo.GetByKey(id) : new SxRedirect();
            var viewModel = Mapper.Map<SxRedirect, SxVMRedirect>(model);
            return View(viewModel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMRedirect model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMRedirect, SxRedirect>(model);
                SxRedirect newModel = null;
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
        public virtual ActionResult Delete(SxRedirect model)
        {
            var data = _repo.GetByKey(model.Id);
            if (data == null)
                return new HttpNotFoundResult();

            _repo.Delete(model);
            return RedirectToAction("Index");
        }
    }
}
