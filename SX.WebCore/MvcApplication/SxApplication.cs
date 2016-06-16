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

        private Action<HttpConfiguration> _configWebApi;
        private Action<RouteCollection> _registerRoutes;
        private static MapperConfiguration _mapperConfiguration;
        private static SxErrorProvider _errorProvider;
        private static SxBannerProvider _bannerProvider;
        private static SxSiteSettingsProvider _siteSettingsProvider;
        private static MemoryCache _cache;
        private static string _logDirectory;
        private static bool _isLogRequests;

        protected SxApplication(
            Action<HttpConfiguration> webApiConfigRegister,
            Action<RouteCollection> registerRoutes,
            MapperConfiguration mapperConfiguration,
            string logDirectory = null,
            bool isLogRequests=false)
        {
            _configWebApi = webApiConfigRegister;
            _registerRoutes = registerRoutes;
            _mapperConfiguration = mapperConfiguration;
            _logDirectory = logDirectory;
            _isLogRequests = isLogRequests;
        }

        public static bool IsLogRequest
        {
            get
            {
                return _isLogRequests;
            }
        }

        public static MemoryCache AppCache
        {
            get
            {
                return _cache;
            }
        }

        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }

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

        protected virtual void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            if (_configWebApi != null)
                GlobalConfiguration.Configure(_configWebApi);

            if (_registerRoutes != null)
                _registerRoutes(RouteTable.Routes);

            _cache = new MemoryCache("APPLICATION_CACHE");

            _logDirectory = _logDirectory ?? "~/Logs";
            var logDirectoryPath = Server.MapPath(_logDirectory);
            if (!Directory.Exists(logDirectoryPath))
                Directory.CreateDirectory(logDirectoryPath);
            _errorProvider = new SxErrorProvider(logDirectoryPath);

            _bannerProvider = new SxBannerProvider(() => GetBanners().Banners);

            _siteSettingsProvider = new SxSiteSettingsProvider(()=>getSiteSettings());
        }

        private static Dictionary<string, SxSiteSetting> getSiteSettings(CacheItemPolicy cip = null)
        {
            var data = (Dictionary<string, SxSiteSetting>)AppCache["CACHE_SITE_SETTINGS"];
            if (data == null)
            {
                cip = cip  ?? _defaultPolicy15Min;
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

        public static SxErrorProvider ErrorProvider
        {
            get
            {
                return _errorProvider;
            }
        }

        public static SxBannerProvider BannerProvider
        {
            get
            {
                return _bannerProvider;
            }
        }

        public static SxSiteSettingsProvider SiteSettingsProvider
        {
            get
            {
                return _siteSettingsProvider;
            }
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
            _errorProvider.WriteMessage(ex);
        }
    }
}
