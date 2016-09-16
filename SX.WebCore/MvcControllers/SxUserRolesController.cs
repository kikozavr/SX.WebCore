using SX.WebCore.Managers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.ViewModels;
using System;
using SX.WebCore.Repositories;
using System.Linq;
using SX.WebCore.MvcControllers.Abstract;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxUserRolesController : SxBaseController
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

        private static SxRepoAppRole _repo=new SxRepoAppRole();
        public static SxRepoAppRole Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _rolePageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async virtual Task<ActionResult> Index(SxVMAppRole filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _rolePageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

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
