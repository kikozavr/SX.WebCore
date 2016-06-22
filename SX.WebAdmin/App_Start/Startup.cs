using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SX.WebAdmin.Infrastructure;
using SX.WebCore;
using SX.WebCore.Managers;
using System;

[assembly: OwinStartup(typeof(SX.WebAdmin.Startup))]
namespace SX.WebAdmin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер
            app.CreatePerOwinContext<DbContext>(DbContext.Create<DbContext>);
            app.CreatePerOwinContext<SxAppUserManager>(SxAppUserManager.Create<DbContext>);
            app.CreatePerOwinContext<SxAppSignInManager>(SxAppSignInManager.Create);
            app.CreatePerOwinContext<SxAppRoleManager>(SxAppRoleManager.Create<DbContext>);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SxAppUserManager, SxAppUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}