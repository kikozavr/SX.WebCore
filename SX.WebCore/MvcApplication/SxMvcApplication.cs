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
    public abstract class SxMvcApplication<TDbContext> : HttpApplication where TDbContext : SxDbContext
    {
        private static readonly string _logDirectoryPath = "~/logs";
        public static DateTime LastStartDate { get; set; }
        public static MemoryCache AppCache { get; set; }
        public static SxErrorProvider ErrorProvider { get; set; }
        public static MapperConfiguration MapperConfiguration { get; set; }

        public static SxBannerProvider BannerProvider { get; set; }
        private static SxVMBannerCollection getBannerCollection(CacheItemPolicy cip = null)
        {
            var cacheBanners = (SxVMBannerCollection)AppCache["CACHE_SITE_BANNERS"];

            if (cacheBanners == null)
            {
                cip = cip ?? SxCacheExpirationManager.GetExpiration(minutes:60);
                cacheBanners = new SxVMBannerCollection();
                cacheBanners.Banners = SxBannersController<TDbContext>.Repo.All;
                cacheBanners.BannerGroups = SxBannerGroupsController<TDbContext>.Repo.All;

                AppCache.Add("CACHE_SITE_BANNERS", cacheBanners, cip);
            }

            return cacheBanners;
        }

        public static SxSiteSettingsProvider SiteSettingsProvider { get; set; }
        private static Dictionary<string, SxSiteSetting> getSiteSettings(CacheItemPolicy cip = null)
        {
            var data = (Dictionary<string, SxSiteSetting>)AppCache["CACHE_SITE_SETTINGS"];
            if (data == null)
            {
                cip = cip ?? SxCacheExpirationManager.GetExpiration(minutes: 60);
                data = SxSiteSettingsController<TDbContext>.Repo.GetAll();
                AppCache.Add("CACHE_SITE_SETTINGS", data, cip);
            }

            return data;
        }

        public static string[] GetBannedUrls(CacheItemPolicy cip = null)
        {
            var data = (string[])AppCache["CACHE_SITE_BANNED_URL"];

            if (data == null)
            {
                cip = cip ?? SxCacheExpirationManager.GetExpiration(minutes: 60);
                data = SxBannedUrlsController<TDbContext>.Repo.GetAllUrls();
                AppCache.Add("CACHE_SITE_BANNED_URL", data, cip);
            }

            return data;
        }

        public static SxShareButton[] ShareButtons
        {
            get
            {
                var list = (SxShareButton[])AppCache.Get("CACHE_LIKE_BUTTONS");
                if (list == null)
                {
                    list = SxShareButtonsController<TDbContext>.Repo.ShareButtonsList;
                    AppCache.Add("CACHE_LIKE_BUTTONS", list, SxCacheExpirationManager.GetExpiration(minutes: 60));
                }
                return list;
            }
        }

        public static Dictionary<string, string> UsersOnSite
        {
            get
            {
                var data = (Dictionary<string, string>)AppCache["CACHE_USERS_ON_SITE"];
                if(data==null)
                {
                    data = new Dictionary<string, string>();
                    AppCache["CACHE_USERS_ON_SITE"] = data;
                }

                return data;
            }
        }

        public static SxSiteNetProvider SiteNetsProvider { get; set; }
        private static SxVMSiteNet[] getSiteNets()
        {
            var data = (SxVMSiteNet[])AppCache["CACHE_SITE_NETS"];
            if (data == null)
            {
                data = SxSiteNetsController<TDbContext>.Repo.SiteNets;
                AppCache.Add("CACHE_SITE_NETS", data, SxCacheExpirationManager.GetExpiration(minutes: 60));
            }

            return data;
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            createLogDirectory();
            AppCache = new MemoryCache("APPLICATION_CACHE");
            ErrorProvider = new SxErrorProvider(Server.MapPath(_logDirectoryPath));

            var args = (SxApplicationEventArgs)e;
            MapperConfiguration = SxAutoMapperConfig.MapperConfigurationInstance(args.MapperConfigurationExpression);
            BannerProvider = new SxBannerProvider(()=>getBannerCollection());
            SiteSettingsProvider = new SxSiteSettingsProvider(() => getSiteSettings());
            SiteNetsProvider = new SxSiteNetProvider(getSiteNets);

            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.SetControllerFactory(new SxControllerFactory<TDbContext>());
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
        public Action<HttpConfiguration> WebApiConfigRegister { get; set; }

        public Action<IMapperConfigurationExpression> MapperConfigurationExpression { get; set; }

        //routes
        public string[] DefaultControllerNamespaces { get; set; }
        public Action<RouteCollection> PreRouteAction { get; set; } = null;
        public Action<RouteCollection> PostRouteAction { get; set; } = null;
    }

}
