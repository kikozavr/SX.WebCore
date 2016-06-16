using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SX.WebCore.Managers
{
    public class SxAppSignInManager : SignInManager<SxAppUser, string>
    {
        public SxAppSignInManager(SxAppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SxAppUser user)
        {
            return user.GenerateUserIdentityAsync((SxAppUserManager)UserManager);
        }

        public static SxAppSignInManager Create(IdentityFactoryOptions<SxAppSignInManager> options, IOwinContext context)
        {
            return new SxAppSignInManager(context.GetUserManager<SxAppUserManager>(), context.Authentication);
        }
    }
}
