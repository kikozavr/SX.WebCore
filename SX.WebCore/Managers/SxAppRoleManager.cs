using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace SX.WebCore.Managers
{
    public class SxAppRoleManager : RoleManager<SxAppRole>
    {
        public SxAppRoleManager(RoleStore<SxAppRole> store)
                : base(store)
        { }

        public static SxAppRoleManager Create<TDbContext>(IdentityFactoryOptions<SxAppRoleManager> options, IOwinContext context) where TDbContext : SxDbContext
        {
            return new SxAppRoleManager(new RoleStore<SxAppRole>(context.Get<TDbContext>()));
        }
    }
}
