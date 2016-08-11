using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxEmployeesController<TDbContext> : SxBaseController<TDbContext> where TDbContext:SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoEmployee<TDbContext> _repo;
        public SxEmployeesController()
        {
            if(_repo==null)
                _repo = new SxRepoEmployee<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "Email", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter)
                .Select(x => Mapper.Map<SxEmployee, SxVMEmployee>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<PartialViewResult> Index(SxVMEmployee filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel =(await _repo.ReadAsync(filter))
                .Select(x => Mapper.Map<SxEmployee, SxVMEmployee>(x))
                .ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string id = null)
        {
            var model = id != null ? _repo.GetByKey(id) : new SxEmployee();
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
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Surname", "Name", "Patronymic", "Description");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}
