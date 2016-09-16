using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SX.WebCore.Managers;
using System;

namespace SX.WebCore.MvcApplication
{
    public class SxOwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<SxDbContext>(SxMvcApplication.GetDbContextInstance);
            app.CreatePerOwinContext<SxAppUserManager>(SxAppUserManager.Create);
            app.CreatePerOwinContext<SxAppSignInManager>(SxAppSignInManager.Create);
            app.CreatePerOwinContext<SxAppRoleManager>(SxAppRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SxAppUserManager, SxAppUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.MapSignalR();
        }

        //private static TDbContext createDbContext(IdentityFactoryOptions<TDbContext> v, IOwinContext context)
        //{
        //    context.Set("owin.DbContext", typeof(TDbContext));
        //    return new TDbContext();
        //}
    }
}
