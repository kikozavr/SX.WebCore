using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System;
using AutoMapper;
using SX.WebCore.Providers;
using System.IO;
using SX.WebCore.Repositories;
using System.Linq;
using System.Collections.Generic;
using SX.WebCore.Resources;

namespace SX.WebCore.MvcApplication
{
    public abstract class SxApplication<TDbContext> : System.Web.HttpApplication where TDbContext : SxDbContext
    {
        private static CacheItemPolicy _defaultPolicy15Min
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
                };
            }
        }

        private static MapperConfiguration _mapperConfiguration;
        public static MapperConfiguration MapperConfiguration { get { return _mapperConfiguration; } }

        public static bool LoggingRequest { get { return (bool)_appCache["APP_LoggingRequest"]; } }

        public static Dictionary<string, string> UsersOnSite
        {
            get
            {
                var data = (Dictionary<string, string>)_appCache["CACHE_USERS_ON_SITE"];
                if(data==null)
                {
                    data = new Dictionary<string, string>();
                    _appCache["CACHE_USERS_ON_SITE"] = data;
                }

                return data;
            }
        }

        public static string SiteDomain
        {
            get
            {
                return (string)AppCache["CACHE_SITE_DOMAIN"];
            }
            set
            {
                AppCache["CACHE_SITE_DOMAIN"] = value;
            }
        }

        private static MemoryCache _appCache;
        public static MemoryCache AppCache { get { return _appCache; } }

        public static string[] GetBannedUrls(CacheItemPolicy cip = null)
        {
            var data = (string[])AppCache["CACHE_SITE_BANNED_URL"];

            if (data == null)
            {
                cip = cip ?? _defaultPolicy15Min;
                data = new SxRepoBannedUrl<TDbContext>().GetAllUrls();
                AppCache.Add("CACHE_SITE_BANNED_URL", data, cip);
            }

            return data;
        }

        public static SxBannerCollection GetBanners(CacheItemPolicy cip = null)
        {
            var cacheBanners = (SxBannerCollection)AppCache["CACHE_SITE_BANNERS"];

            if (cacheBanners == null)
            {
                cip = cip ?? _defaultPolicy15Min;
                cacheBanners = new SxBannerCollection();
                cacheBanners.Banners = new SxRepoBanner<TDbContext>().All.ToArray();
                cacheBanners.BannerGroups = new SxRepoBannerGroup<TDbContext>().All.ToArray();

                AppCache.Add("CACHE_SITE_BANNERS", cacheBanners, _defaultPolicy15Min);
            }

            return cacheBanners;
        }

        private static SxErrorProvider _errorProvider;
        public static SxErrorProvider ErrorProvider { get { return _errorProvider; } }

        private static SxBannerProvider _bannerProvider;
        public static SxBannerProvider BannerProvider { get { return _bannerProvider; } }

        private static SxSiteSettingsProvider _siteSettingsProvider;
        public static SxSiteSettingsProvider SiteSettingsProvider { get { return _siteSettingsProvider; } }
        private static Dictionary<string, SxSiteSetting> getSiteSettings(CacheItemPolicy cip = null)
        {
            var data = (Dictionary<string, SxSiteSetting>)AppCache["CACHE_SITE_SETTINGS"];
            if (data == null)
            {
                cip = cip ?? _defaultPolicy15Min;
                data = new SxRepoSiteSetting<TDbContext>().GetByKeys(
                        Settings.siteName,
                        Settings.siteLogoPath,
                        Settings.siteBgPath,
                        Settings.emptyGameIconPath,
                        Settings.emptyGameGoodImagePath,
                        Settings.emptyGameBadImagePath,
                        Settings.robotsFileSetting,
                        Settings.siteFaveiconPath
                    );
                AppCache.Add("CACHE_SITE_SETTINGS", data, cip);
            }

            return data;
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            _appCache = new MemoryCache("APPLICATION_CACHE");

            var args = (SxApplicationEventArgs)e;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(args.WebApiConfigRegister);
            args.RegisterRoutes(RouteTable.Routes);

            _mapperConfiguration = args.MapperConfiguration;

            var logDirectoryPath = Server.MapPath(args.LogDirectory ?? "~/Logs");
            if (!Directory.Exists(logDirectoryPath))
                Directory.CreateDirectory(logDirectoryPath);
            _errorProvider = new SxErrorProvider(logDirectoryPath);

            Context.Cache["APP_LoggingRequest"]=args.LoggingRequest;

            _bannerProvider = new SxBannerProvider(() => GetBanners().Banners);

            _siteSettingsProvider = new SxSiteSettingsProvider(() => getSiteSettings());
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

    public class SxApplicationEventArgs : EventArgs
    {
        public Action<HttpConfiguration> WebApiConfigRegister { get; set; }
        public Action<RouteCollection> RegisterRoutes { get; set; }
        public MapperConfiguration MapperConfiguration { get; set; }
        public string LogDirectory { get; set; }
        public bool LoggingRequest { get; set; }
    }

}
