using SX.WebCore.MvcApplication;
using System;
using System.Data.Entity;

namespace SX.WebAdmin
{
    public class MvcApplication : SxApplication<Infrastructure.DbContext>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {

            var args = new SxApplicationEventArgs();
            args.WebApiConfigRegister = WebApiConfig.Register;
            args.RegisterRoutes = RouteConfig.RegisterRoutes;
            args.MapperConfiguration = AutoMapperConfig.MapperConfigurationInstance;
            args.LogDirectory = null;
            args.LoggingRequest = false;

            Database.SetInitializer<Infrastructure.DbContext>(null);

            base.Application_Start(sender, args);
        }
    }
}
