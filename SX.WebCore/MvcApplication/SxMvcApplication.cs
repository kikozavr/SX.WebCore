using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System;
using AutoMapper;
using SX.WebCore.Providers;
using System.IO;
using System.Collections.Generic;
using System.Web;
using SX.WebCore.Managers;
using SX.WebCore.ViewModels;
using SX.WebCore.RazorViewEngine;
using SX.WebCore.MvcControllers;
using SX.WebCore.MvcControllerFactory;

namespace SX.WebCore.MvcApplication
{
    public abstract class SxMvcApplication : HttpApplication
    {
        public static Func<SxDbContext> GetDbContextInstance { get; set; }
        private static readonly string _logDirectoryPath = "~/logs";
        public static DateTime LastStartDate { get; set; }
        public static SxCacheProvider CacheProvider { get; set; }
        public static SxErrorProvider ErrorProvider { get; set; }
        public static MapperConfiguration MapperConfiguration { get; set; }

        public static SxBannerProvider BannerProvider { get; set; }
        private static SxVMBannerCollection getBannerCollection()
        {
            var cacheBanners = CacheProvider.Get<SxVMBannerCollection>("CACHE_SITE_BANNERS");

            if (cacheBanners == null)
            {
                cacheBanners = new SxVMBannerCollection();
                cacheBanners.Banners = SxBannersController.Repo.All;
                cacheBanners.BannerGroups = SxBannerGroupsController.Repo.All;

                CacheProvider.Set("CACHE_SITE_BANNERS", cacheBanners, 60);
            }

            return cacheBanners;
        }

        public static SxSiteSettingsProvider SiteSettingsProvider{ get; set; }
        private static Dictionary<string, SxSiteSetting> getSiteSettings()
        {
            var data = CacheProvider.Get<Dictionary<string, SxSiteSetting>>("CACHE_SITE_SETTINGS");
            if (data == null)
            {
                data = SxSiteSettingsController.Repo.GetAll();
                CacheProvider.Set("CACHE_SITE_SETTINGS", data, 60);
            }

            return data;
        }

        public static string[] GetBannedUrls()
        {
            var data = CacheProvider.Get<string[]>("CACHE_SITE_BANNED_URL");

            if (data == null)
            {
                data = SxBannedUrlsController.Repo.GetAllUrls();
                CacheProvider.Set("CACHE_SITE_BANNED_URL", data, 60);
            }

            return data;
        }

        public static SxShareButton[] ShareButtons
        {
            get
            {
                var data = CacheProvider.Get<SxShareButton[]>("CACHE_SHARE_BUTTONS");

                if (data == null)
                {
                    data = SxShareButtonsController.Repo.ShareButtonsList;
                    CacheProvider.Set("CACHE_SHARE_BUTTONS", data, 60);
                }

                return data;
            }
        }

        public static Dictionary<string, string> UsersOnSite
        {
            get
            {
                var data = CacheProvider.Get<Dictionary<string, string>>("CACHE_USERS_ON_SITE");
                if(data==null)
                {
                    data = new Dictionary<string, string>();
                    CacheProvider.Set("CACHE_USERS_ON_SITE", data, int.MaxValue);
                }

                return data;
            }
        }

        public static SxSiteNetProvider SiteNetsProvider { get; set; }
        private static SxVMSiteNet[] getSiteNets()
        {
            var data = CacheProvider.Get<SxVMSiteNet[]>("CACHE_SITE_NETS");
            if (data == null)
            {
                data = SxSiteNetsController.Repo.SiteNets;
                CacheProvider.Set("CACHE_SITE_NETS", data, 60);
            }

            return data;
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            createLogDirectory();
            CacheProvider = new SxCacheProvider();
            ErrorProvider = new SxErrorProvider(Server.MapPath(_logDirectoryPath));

            var args = (SxApplicationEventArgs)e;
            if (args.GetDbContextInstance == null)
                throw new ArgumentNullException("Необходимо задать функцию получения контекста БД");
            GetDbContextInstance = args.GetDbContextInstance;
            MapperConfiguration = SxAutoMapperConfig.MapperConfigurationInstance(args.MapperConfigurationExpression);
            BannerProvider = new SxBannerProvider(()=>getBannerCollection());
            SiteSettingsProvider = new SxSiteSettingsProvider(() => getSiteSettings());
            SiteNetsProvider = new SxSiteNetProvider(getSiteNets);

            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.SetControllerFactory(new SxControllerFactory());
            GlobalConfiguration.Configure(args.WebApiConfigRegister);
            SxRouteConfig.RegisterRoutes(RouteTable.Routes, args.DefaultControllerNamespaces, args.PreRouteAction, args.PostRouteAction);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SxRazorViewEngine());

            LastStartDate = DateTime.Now;

        }
        private void createLogDirectory()
        {
            var logDirectoryPath = Server.MapPath(_logDirectoryPath);
            if (!Directory.Exists(logDirectoryPath))
                Directory.CreateDirectory(logDirectoryPath);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            ErrorProvider.WriteMessage(ex);
        }
    }

    public sealed class SxApplicationEventArgs : EventArgs
    {
        public Func<SxDbContext> GetDbContextInstance { get; set; }
        public Action<HttpConfiguration> WebApiConfigRegister { get; set; }

        public Action<IMapperConfigurationExpression> MapperConfigurationExpression { get; set; }

        //routes
        public string[] DefaultControllerNamespaces { get; set; }
        public Action<RouteCollection> PreRouteAction { get; set; } = null;
        public Action<RouteCollection> PostRouteAction { get; set; } = null;
    }

}
