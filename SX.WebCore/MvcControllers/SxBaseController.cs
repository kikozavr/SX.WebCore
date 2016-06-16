using AutoMapper;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxBaseController<TDbContext> : Controller where TDbContext : SxDbContext
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

        private static IMapper _mapper;
        protected IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public SxBaseController()
        {
            if (_mapper == null)
                _mapper = SxApplication<TDbContext>.MapperConfiguration.CreateMapper();
        }

        public string SxAreaName { get; set; }
        public string SxControllerName { get; set; }
        public string SxActionName { get; set; }
        public string SxSessionId { get; set; }
        public string SxRawUrl { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeDataValues = filterContext.RequestContext.RouteData.Values;
            SxAreaName = (string)routeDataValues["area"];
            SxControllerName = (string)routeDataValues["controller"];
            SxActionName = (string)routeDataValues["action"];
            var session = filterContext.RequestContext.HttpContext.Session;
            SxSessionId = session?.SessionID;
            SxRawUrl = Request.RawUrl.ToLower();

            if(!SxApplication<SxDbContext>.IsLogRequest)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            //забаненные адреса
            var urlRef = Request.UrlReferrer;
            if (urlRef != null)
            {
                if (SxApplication<TDbContext>.GetBannedUrls().Contains(urlRef.ToString()))
                {
                    filterContext.Result = new HttpStatusCodeResult(403);
                    return;
                }
            }

            //если экшн является дочерним или задан аттрибут нелогирования запроса
            var notLogRequest = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(NotLogRequestAttribute)) != null;
            if (filterContext.IsChildAction || notLogRequest) return;

            //редирект, если есть
            var redirect = getRedirect();
            if(redirect != null && redirect.NewUrl!=null)
            {
                filterContext.Result = new RedirectResult(redirect.NewUrl);
                return;
            }

            //записываем теги seo
            writePageSeoinfo();

            //пишем баннеры страницы
            writePageBanners();

            //пишем информацию о запросе
            writeRequestInfo();

            base.OnActionExecuting(filterContext);
        }

        private SxRedirect getRedirect(CacheItemPolicy cip = null)
        {
            cip = cip ?? _defaultPolicy15Min;
            var redirect = (SxRedirect)SxApplication<TDbContext>.AppCache["CACHE_REDIRECT_"+SxRawUrl];
            if(redirect==null)
            {
                redirect = new SxRepoRedirect<TDbContext>().GetRedirect(SxRawUrl);
                SxApplication<TDbContext>.AppCache.Add("CACHE_REDIRECT_" + SxRawUrl, redirect, cip);
            }

            return redirect;
        }

        private SxSeoInfo getPageSeoInfo(CacheItemPolicy cip = null)
        {
            var seoInfo = (SxSeoInfo)SxApplication<TDbContext>.AppCache["CACHE_SEOINFO_" + SxRawUrl];
            if(seoInfo==null)
            {
                seoInfo = new SxRepoSeoInfo<TDbContext>().GetSeoInfo(SxRawUrl);
                SxApplication<TDbContext>.AppCache.Add("CACHE_SEOINFO_" + SxRawUrl, seoInfo, cip);
            }
            return seoInfo;
        }
        private void writePageSeoinfo()
        {
            var seoInfo = getPageSeoInfo();
            if (seoInfo == null || seoInfo.SeoTitle == null) return;

            ViewBag.Title = seoInfo.SeoTitle;
            ViewBag.Description = seoInfo.SeoDescription;
            if(seoInfo.Keywords.Any())
            {
                var sb = new StringBuilder();
                foreach (var k in seoInfo.Keywords)
                {
                    sb.AppendFormat(",{0}", k.Value);
                }
                sb.Remove(0, 1);

                ViewBag.Keywords = sb.ToString();
            }
            ViewBag.H1 = seoInfo.H1;
            ViewBag.H1CssClass = seoInfo.H1CssClass;
        }

        private void writeRequestInfo()
        {
            if (Request.IsLocal) return;

            Task.Run(() =>
            {
                var requestInstance = new SxRequest
                {
                    Browser = Request.Browser != null ? Request.Browser.Browser : null,
                    ClientIP = Request.ServerVariables["REMOTE_ADDR"],
                    RawUrl = Request.RawUrl.ToLowerInvariant(),
                    RequestType = Request.RequestType,
                    UrlRef = Request.UrlReferrer != null ? Request.UrlReferrer.ToString().ToLowerInvariant() : null,
                    SessionId = Request.RequestContext.HttpContext.Session.SessionID,
                    UserAgent = Request.UserAgent
                };
                new SxRepoRequest<TDbContext>().Create(requestInstance);
            });
        }

        private void writePageBanners()
        {
            ViewBag.PageBanners = SxApplication<TDbContext>.BannerProvider.GetPageBanners(SxControllerName, SxActionName);
        }
    }
}
