using SX.WebCore.Managers;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SX.WebCore.ViewModels;
using SX.WebCore.MvcApplication;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxUsersController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
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
    }
}
