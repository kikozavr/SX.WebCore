using AutoMapper;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using System;
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

        protected static IMapper Mapper { get; set; }

        private static Action<SxBaseController<TDbContext>> _writeBreadcrumbs;
        protected static Action<SxBaseController<TDbContext>> WriteBreadcrumbs
        {
            set
            {
                _writeBreadcrumbs = value;
            }
        }

        public SxBaseController()
        {
            if (Mapper == null)
                Mapper = SxApplication<TDbContext>.MapperConfiguration.CreateMapper();
        }

        public string SxAreaName { get; set; }
        public string SxControllerName { get; set; }
        public string SxActionName { get; set; }
        public string SxSessionId { get; set; }
        public string SxRawUrl { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeDataValues = filterContext.RequestContext.RouteData.Values;
            SxAreaName = routeDataValues["area"]?.ToString().ToLower();
            SxControllerName = routeDataValues["controller"].ToString().ToLower();
            SxActionName = routeDataValues["action"].ToString().ToLower();
            var session = filterContext.RequestContext.HttpContext.Session;
            SxSessionId = session?.SessionID;
            SxRawUrl = Request.RawUrl.ToLower();

            //забаненные адреса (не используется)
            //var urlRef = Request.UrlReferrer;
            //if (urlRef != null)
            //{
            //    if (SxApplication<TDbContext>.GetBannedUrls().Contains(urlRef.ToString()))
            //    {
            //        filterContext.Result = new HttpStatusCodeResult(403);
            //        return;
            //    }
            //}

            //если экшн является дочерним или задан аттрибут нелогирования запроса
            var notLogRequest = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(NotLogRequestAttribute)) != null;
            if (filterContext.IsChildAction || notLogRequest) return;

            //редирект, если есть
            var redirect = get301Redirect();
            if (redirect != null && redirect.NewUrl != null)
            {
                filterContext.Result = new RedirectResult(redirect.NewUrl);
                return;
            }

            //записываем теги seo
            writePageSeoTags();

            //пишем баннеры страницы
            writePageBanners();

            //пишем информацию о запросе
            if (Equals(filterContext.HttpContext.Cache["APP_LoggingRequest"],true) && SxAreaName!="admin")
            {
                writeRequestInfo();
            }

            //пишем хлебные крошки
            if (_writeBreadcrumbs!=null)
                _writeBreadcrumbs(this);

            base.OnActionExecuting(filterContext);
        }

        private Sx301Redirect get301Redirect(CacheItemPolicy cip = null)
        {
            cip = cip ?? _defaultPolicy15Min;
            var redirect = (Sx301Redirect)SxApplication<TDbContext>.AppCache["CACHE_REDIRECT_"+SxRawUrl];
            if(redirect==null)
            {
                redirect = new SxRepo301Redirect<TDbContext>().Get301Redirect(SxRawUrl);
                SxApplication<TDbContext>.AppCache.Add("CACHE_REDIRECT_" + SxRawUrl, redirect, cip);
            }

            return redirect;
        }

        private SxSeoTags getPageSeoTags(CacheItemPolicy cip = null)
        {
            var seoTags = (SxSeoTags)SxApplication<TDbContext>.AppCache["CACHE_SEOTAGS_" + SxRawUrl];
            if(seoTags==null)
            {
                seoTags = new SxRepoSeoTags<TDbContext>().GetSeoTags(SxRawUrl);
                SxApplication<TDbContext>.AppCache.Add("CACHE_SEOTAGS_" + SxRawUrl, seoTags, cip);
            }
            return seoTags;
        }
        private void writePageSeoTags()
        {
            var seoTags = getPageSeoTags();
            if (seoTags == null || seoTags.SeoTitle == null) return;

            ViewBag.Title = seoTags.SeoTitle;
            ViewBag.Description = seoTags.SeoDescription;
            if(seoTags.Keywords.Any())
            {
                var sb = new StringBuilder();
                foreach (var k in seoTags.Keywords)
                {
                    sb.AppendFormat(",{0}", k.Value);
                }
                sb.Remove(0, 1);

                ViewBag.Keywords = sb.ToString();
            }
            ViewBag.H1 = seoTags.H1;
            ViewBag.H1CssClass = seoTags.H1CssClass;
        }

        private void writeRequestInfo()
        {
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
