using SX.WebCore.Managers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Linq;
using SX.WebCore.ViewModels;
using System;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxUserRolesController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _rolePageSize = 10;

        private SxAppRoleManager _roleManager;
        private SxAppRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<SxAppRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _rolePageSize) { Order = order };
            var data = RoleManager.Roles.OrderBy(x => x.Name).ToArray();
            filter.PagerInfo.TotalItems = data.Count();
            var viewModel = data
                .Select(x => Mapper.Map<SxAppRole, SxVMAppRole>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMAppRole filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _rolePageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            var data = RoleManager.Roles.OrderBy(x => x.Name).ToArray();

            if (!string.IsNullOrEmpty(filterModel.Name))
                data = data.Where(x => x.Name.ToUpper().Contains(filterModel.Name.ToUpper())).ToArray();
            if (!string.IsNullOrEmpty(filterModel.Description))
                data = data.Where(x => x.Description.ToUpper().Contains(filterModel.Description.ToUpper())).ToArray();

            if (order != null && !Equals(order.Direction, SortDirection.Unknown))
            {
                switch (order.FieldName)
                {
                    case "Name":
                        if (Equals(order.Direction, SortDirection.Asc))
                            data = data.OrderBy(x => x.Name).ToArray();
                        else
                            data = data.OrderByDescending(x => x.Name).ToArray();
                        break;

                    case "Description":
                        if (Equals(order.Direction, SortDirection.Asc))
                            data = data.OrderBy(x => x.Description).ToArray();
                        else
                            data = data.OrderByDescending(x => x.Description).ToArray();
                        break;
                }
            }
            else
            {
                data = data.OrderBy(x => x.Name).ToArray();
                filter.Order = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            }

            filter.PagerInfo.TotalItems = data.Count();
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _rolePageSize ? 1 : page;
            var viewModel = data
                .Select(x => Mapper.Map<SxAppRole, SxVMAppRole>(x))
                .ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual async Task<ViewResult> Edit(string id = null)
        {
            var model = !string.IsNullOrEmpty(id)
                ? await RoleManager.FindByIdAsync(id)
                : new SxAppRole { Id = null };

            var viewModel = Mapper.Map<SxAppRole, SxVMAppRole>(model);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(SxVMAppRole model)
        {
            var isNew = string.IsNullOrEmpty(model.Id);
            if (isNew)
            {
                if (await RoleManager.FindByNameAsync(model.Name) != null)
                    ModelState.AddModelError("Name", "Роль с таким именем уже добавлена в БД");
            }

            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMAppRole, SxAppRole>(model);
                if (isNew)
                {
                    redactModel.Id = Guid.NewGuid().ToString();
                    await RoleManager.CreateAsync(redactModel);
                }
                else
                {
                    var oldRole = await RoleManager.FindByIdAsync(model.Id);
                    oldRole.Name = model.Name;
                    oldRole.Description = model.Description;
                    await RoleManager.UpdateAsync(oldRole);
                }
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxAppRole model)
        {
            var role = await RoleManager.FindByIdAsync(model.Id);
            if (role != null)
                await RoleManager.DeleteAsync(role);

            return RedirectToAction("index");
        }
    }
}
