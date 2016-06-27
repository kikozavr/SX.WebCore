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

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxUsersController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoAppUser<TDbContext> _repo;
        private static SxRepoEmployee<TDbContext> _repoEmployee;
        public SxUsersController()
        {
            if (_repo == null)
                _repo = new SxRepoAppUser<TDbContext>();
            if (_repoEmployee == null)
                _repoEmployee = new SxRepoEmployee<TDbContext>();
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
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "NikName", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var users = UserManager.Users;
            var roles = RoleManager.Roles.Select(x => new { RoleId = x.Id, RoleName = x.Name }).ToArray();
            var usersOnline = SxApplication<TDbContext>.UsersOnSite;

            var viewModel = users.OrderByDescending(x => x.DateCreate).Skip((page - 1) * _pageSize).Take(_pageSize).ToArray()
                .Select(x => new SxVMAppUser
                {
                    Id = x.Id,
                    Email = x.Email,
                    NikName = x.NikName,
                    Roles = x.Roles.Select(r => new SxVMAppRole { Id = r.RoleId }).ToArray(),
                    IsOnline = usersOnline.ContainsValue(x.Email)
                }).ToArray();

            for (int i = 0; i < viewModel.Length; i++)
            {
                for (int y = 0; y < viewModel[i].Roles.Length; y++)
                {
                    viewModel[i].Roles[y].Name = roles.FirstOrDefault(r => r.RoleId == viewModel[i].Roles[y].Id).RoleName;
                }
            }

            filter.PagerInfo.TotalItems = users.Count();
            filter.PagerInfo.Page = viewModel.Length <= _pageSize ? 1 : page;
            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMAppUser filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var users = UserManager.Users;
            if (!string.IsNullOrEmpty(filterModel.NikName))
                users = users.Where(x => x.NikName.Contains(filterModel.NikName));
            if (!string.IsNullOrEmpty(filterModel.Email))
                users = users.Where(x => x.Email.Contains(filterModel.Email));

            var roles = RoleManager.Roles.Select(x => new { RoleId = x.Id, RoleName = x.Name }).ToArray();
            var usersOnline = SxApplication<TDbContext>.UsersOnSite;

            var viewModel = users.OrderByDescending(x => x.DateCreate).Skip((page - 1) * _pageSize).Take(_pageSize).ToArray()
                .Select(x => new SxVMAppUser
                {
                    Id = x.Id,
                    Email = x.Email,
                    NikName = x.NikName,
                    Roles = x.Roles.Select(r => new SxVMAppRole { Id = r.RoleId }).ToArray(),
                    IsOnline = usersOnline.ContainsValue(x.Email)
                }).ToArray();

            for (int i = 0; i < viewModel.Length; i++)
            {
                for (int y = 0; y < viewModel[i].Roles.Length; y++)
                {
                    viewModel[i].Roles[y].Name = roles.FirstOrDefault(r => r.RoleId == viewModel[i].Roles[y].Id).RoleName;
                }
            }

            filter.PagerInfo.TotalItems = users.Count();
            filter.PagerInfo.Page = viewModel.Length <= _pageSize ? 1 : page;
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public PartialViewResult UsersOnSite()
        {
            var emails = SxApplication<TDbContext>.UsersOnSite.Select(x => x.Value).Distinct().ToArray();
            var data = _repo.GetUsersByEmails(emails);
            var viewModel = data.Select(x => Mapper.Map<SxAppUser, SxVMAppUser>(x)).ToArray();

            return PartialView("~/views/users/_usersonsite.cshtml", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string id = null)
        {
            var data = UserManager.FindById(id);
            var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray();
            ViewBag.Roles = allRoles;
            var viewModel = getEditUser(data, allRoles);
            if (viewModel.Avatar != null)
                ViewBag.PictureCaption = viewModel.Avatar.Caption;

            return View(viewModel);
        }

        private SxVMEditAppUser getEditUser(SxAppUser data, SxAppRole[] allRoles)
        {
            var editUser = new SxVMEditAppUser
            {
                Id = data.Id,
                Avatar = data.Avatar,
                AvatarId = data.AvatarId,
                Email = data.Email,
                NikName = data.NikName,
                IsOnline = SxApplication<TDbContext>.UsersOnSite.ContainsValue(data.UserName),
                IsEmployee = _repoEmployee.GetByKey(data.Id) != null
            };

            editUser.Roles = data.Roles.Join(allRoles, u => u.RoleId, r => r.Id, (u, r) => new SxVMAppRole
            {
                Id = u.RoleId,
                Name = r.Name,
                Description = r.Description
            }).ToArray();

            return editUser;
        }

        private void addEmployee(SxVMEditAppUser model)
        {
            var redactModel = Mapper.Map<SxVMEditAppUser, SxEmployee>(model);
            var data = _repoEmployee.Create(redactModel);
        }

        public void delEmployee(SxVMEditAppUser model)
        {
            _repoEmployee.Delete(model.Id);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditRoles(string userId)
        {
            var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray();
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
                TempData["UserRoleMessage"] = "Роли успешно заданы";
                data = UserManager.FindById(userId);
            }

            var viewModel = getEditUser(data, allRoles);
            return PartialView("~/views/Users/_UserRoles.cshtml", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditUserInfo(SxVMEditAppUser user)
        {
            if (ModelState.IsValid)
            {
                var oldUser = UserManager.FindById(user.Id);
                oldUser.NikName = user.NikName;
                oldUser.AvatarId = user.AvatarId;
                UserManager.Update(oldUser);

                if (user.IsEmployee)
                    addEmployee(user);
                else if (!user.IsEmployee)
                    delEmployee(user);

                var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray();
                var viewModel = getEditUser(oldUser, allRoles);
                if (viewModel.Avatar != null)
                    ViewBag.PictureCaption = viewModel.Avatar.Caption;
                TempData["UserInfoMessage"] = "Информация обновлена";
                return PartialView("~/views/Users/_UserInfo.cshtml", viewModel);
            }
            else
                return PartialView("~/views/Users/_UserInfo.cshtml", user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult EditUserReport(int[] reportsId)
        {
            if (ModelState.IsValid)
            {
                TempData["UserInfoMessage"] = "Информация обновлена";
                return PartialView("~/views/Users/_UserReports.cshtml", reportsId);
            }
            else
                return PartialView("~/views/Users/_UserReports.cshtml", reportsId);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(SxVMEditAppRole model)
        {
            return RedirectToAction("index");
            throw new NotImplementedException();
        }
    }
}
