using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System;
using AutoMapper;
using SX.WebCore.Providers;
using System.IO;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using System.Web;
using SX.WebCore.Managers;
using SX.WebCore.ViewModels;

namespace SX.WebCore.MvcApplication
{
    public abstract class SxApplication<TDbContext> : HttpApplication where TDbContext : SxDbContext
    {
        private static readonly string _logDirectoryPath = "~/logs";
        public static DateTime LastStartDate { get; set; }
        public static MemoryCache AppCache { get; set; }
        public static SxErrorProvider ErrorProvider { get; set; }
        public static bool LoggingRequest { get; set; }
        public static MapperConfiguration MapperConfiguration { get; set; }

        public static SxBannerProvider BannerProvider { get; set; }
        private static SxVMBannerCollection getBannerCollection(CacheItemPolicy cip = null)
        {
            var cacheBanners = (SxVMBannerCollection)AppCache["CACHE_SITE_BANNERS"];

            if (cacheBanners == null)
            {
                cip = cip ?? SxCacheExpirationManager.GetExpiration(minutes:60);
                cacheBanners = new SxVMBannerCollection();
                cacheBanners.Banners = new SxRepoBanner<TDbContext>().All;
                cacheBanners.BannerGroups = new SxRepoBannerGroup<TDbContext>().All;

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
                data = new SxRepoSiteSetting<TDbContext>().GetAll();
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
                data = new SxRepoBannedUrl<TDbContext>().GetAllUrls();
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
                    list = new SxRepoShareButton<TDbContext>().ShareButtonsList;
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

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            createLogDirectory();
            AppCache = new MemoryCache("APPLICATION_CACHE");
            ErrorProvider = new SxErrorProvider(Server.MapPath(_logDirectoryPath));

            var args = (SxApplicationEventArgs)e;
            LoggingRequest = args.LoggingRequest;
            MapperConfiguration = args.MapperConfiguration;
            BannerProvider= new SxBannerProvider(()=>getBannerCollection());
            SiteSettingsProvider = new SxSiteSettingsProvider(() => getSiteSettings());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(args.WebApiConfigRegister);
            args.RegisterRoutes(RouteTable.Routes);

            LastStartDate = DateTime.Now;

        }
        private void createLogDirectory()
        {
            var logDirectoryPath = Server.MapPath(_logDirectoryPath);
            if (!Directory.Exists(logDirectoryPath))
                Directory.CreateDirectory(logDirectoryPath);
        }

        protected virtual void Session_Start()
        {

        }

        protected virtual void Session_End()
        {

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
        public Action<RouteCollection> RegisterRoutes { get; set; }
        public MapperConfiguration MapperConfiguration { get; set; }
        public bool LoggingRequest { get; set; }
    }

}
