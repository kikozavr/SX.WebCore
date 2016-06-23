using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxEmployeesController<TDbContext> : SxBaseController<TDbContext> where TDbContext:SxDbContext
    {
        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var repo = new SxRepoEmployee<TDbContext>();
            var order = new SxOrder { FieldName = "Email", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = repo.Count(filter);
            var data = repo.Query(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxEmployee, SxVMEmployee>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMEmployee filterModel, SxOrder order, int page = 1)
        {
            var repo = new SxRepoEmployee<TDbContext>();
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            var data = repo.Query(filter);
            var viewModel = data
                .Select(x => Mapper.Map<SxEmployee, SxVMEmployee>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string id = null)
        {
            var model = id != null ? new SxRepoEmployee<TDbContext>().GetByKey(id) : new SxEmployee();
            return View(Mapper.Map<SxEmployee, SxVMEditEmployee>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditEmployee model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMEditEmployee, SxEmployee>(model);
                SxEmployee newModel = null;
                if (model.Id == null)
                    newModel = new SxRepoEmployee<TDbContext>().Create(redactModel);
                else
                    newModel = new SxRepoEmployee<TDbContext>().Update(redactModel, true, "Surname", "Name", "Patronymic", "Description");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}
