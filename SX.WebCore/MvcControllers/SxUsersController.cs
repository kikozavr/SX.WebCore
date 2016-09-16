using SX.WebCore.Managers;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SX.WebCore.ViewModels;
using SX.WebCore.MvcApplication;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using SX.WebCore.MvcControllers.Abstract;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxUsersController : SxBaseController
    {
        private static SxRepoAppUser _repo=new SxRepoAppUser();
        public static SxRepoAppUser Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static readonly string _architectRole = "architect";
        private SxAppUserManager _userManager;
        private SxAppRoleManager _roleManager;
        private SxAppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<SxAppUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
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
        private static int _pageSize = 10;

        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "NikName", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            if (viewModel.Any())
                fillUserStatuses(viewModel);

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(SxVMAppUser filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            if (viewModel.Any())
                fillUserStatuses(viewModel);

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }
        private void fillUserStatuses(SxVMAppUser[] users)
        {
            SxVMAppUser item = null;
            var usersOnSite = SxMvcApplication.UsersOnSite;
            if (!usersOnSite.Any()) return;

            for (int i = 0; i < users.Length; i++)
            {
                item = users[i];
                item.IsOnline = usersOnSite.ContainsValue(item.Email);
            }
        }

        [HttpGet, AllowAnonymous]
        public PartialViewResult UsersOnSite()
        {
            var emails = SxMvcApplication.UsersOnSite.Select(x => x.Value).Distinct().ToArray();
            var data = _repo.GetUsersByEmails(emails);
            var viewModel = data.Select(x => Mapper.Map<SxAppUser, SxVMAppUser>(x)).ToArray();

            return PartialView("_UsersOnSite", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string id = null)
        {
            var data = UserManager.FindById(id);
            var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray().Select(x=>Mapper.Map<SxAppRole, SxVMAppRole>(x)).ToArray();
            ViewBag.Roles = allRoles;
            var viewModel = getEditUser(data, allRoles);
            if (viewModel.Avatar != null)
                ViewBag.AvatarIdCaption = viewModel.Avatar.Caption;

            return View(viewModel);
        }

        private SxVMAppUser getEditUser(SxAppUser data, SxVMAppRole[] allRoles)
        {
            var editUser = new SxVMAppUser
            {
                Id = data.Id,
                Avatar = Mapper.Map<SxPicture, SxVMPicture>(data.Avatar),
                AvatarId = data.AvatarId,
                Email = data.Email,
                NikName = data.NikName,
                IsOnline = SxMvcApplication.UsersOnSite.ContainsValue(data.UserName),
                IsEmployee = SxEmployeesController.Repo.GetByKey(data.Id)!=null,
                Description=data.Description
            };

            editUser.Roles = data.Roles.Join(allRoles, u => u.RoleId, r => r.Id, (u, r) => new SxVMAppRole
            {
                Id = u.RoleId,
                Name = r.Name,
                Description = r.Description
            }).ToArray();

            return editUser;
        }

        private void addEmployee(SxVMAppUser model)
        {
            var redactModel = Mapper.Map<SxVMAppUser, SxEmployee>(model);
            var data = SxEmployeesController.Repo.Create(redactModel);
        }

        public void delEmployee(SxVMAppUser model)
        {
            SxEmployeesController.Repo.Delete(new SxEmployee { Id=model.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditRoles(string userId)
        {
            var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray().Select(x=>Mapper.Map<SxAppRole, SxVMAppRole>(x)).ToArray();
            ViewBag.Roles = allRoles;

            var roles = Request.Form.GetValues("role");
            var data = UserManager.FindById(userId);
            var userRoles = data.Roles.Join(allRoles, r1 => r1.RoleId, r2 => r2.Id, (r1, r2) => new { Id = r1.RoleId, Name = r2.Name })
                .Where(x => x.Name != _architectRole).ToArray();
            List<string> rolesForDelete = new List<string>();
            List<string> rolesForAdd = new List<string>();
            for (int i = 0; i < userRoles.Length; i++)
            {
                var userRole = userRoles[i];
                if (roles.SingleOrDefault(x => x == userRole.Name) == null)
                    rolesForDelete.Add(userRole.Name);
            }

            for (int i = 0; i < roles.Length; i++)
            {
                var role = roles[i];
                if (userRoles.SingleOrDefault(x => x.Name == role) == null)
                    rolesForAdd.Add(role);
            }

            if (rolesForDelete.Any())
            {
                UserManager.RemoveFromRoles(userId, rolesForDelete.ToArray());
            }

            if (rolesForAdd.Any())
            {
                UserManager.AddToRoles(userId, rolesForAdd.ToArray());
            }

            if (rolesForDelete.Any() || rolesForAdd.Any())
            {
                ViewBag.UserRoleMessage = "Роли успешно заданы";
                data = UserManager.FindById(userId);
            }

            var viewModel = getEditUser(data, allRoles);
            return PartialView("_UserRoles", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditUserInfo(SxVMAppUser user)
        {
            if (ModelState.IsValid)
            {
                var oldUser = UserManager.FindById(user.Id);
                oldUser.NikName = user.NikName;
                oldUser.AvatarId = user.AvatarId;
                oldUser.Description = user.Description;
                UserManager.Update(oldUser);

                if (user.IsEmployee)
                    addEmployee(user);
                else if (!user.IsEmployee)
                    delEmployee(user);

                var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray().Select(x => Mapper.Map<SxAppRole, SxVMAppRole>(x)).ToArray();
                var viewModel = getEditUser(oldUser, allRoles);
                if (viewModel.Avatar != null)
                    ViewData["AvatarIdCaption"] = viewModel.Avatar.Caption;
                ViewBag.UserRoleMessage = "Информация обновлена";
                return PartialView("_UserInfo", viewModel);
            }
            else
                return PartialView("_UserInfo", user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditUserReport(int[] reportsId)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserRoleMessage = "Информация обновлена";
                return PartialView("_UserReports", reportsId);
            }
            else
                return PartialView("_UserReports", reportsId);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxAppRole model)
        {
            return RedirectToAction("Index");
            throw new NotImplementedException();
        }
    }
}
