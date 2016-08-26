using AutoMapper;
using Newtonsoft.Json;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SX.WebCore.MvcControllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class SxBaseController<TDbContext> : Controller where TDbContext : SxDbContext
    {
        private static SxRepoAffiliateLink<TDbContext> _repoAffiliateLink;
        private static SxRepoRequest<TDbContext> _repoRequest;

        private static SxRepoStatistic<TDbContext> _repoStatistic;
        protected static SxRepoStatistic<TDbContext> RepoStatistic
        {
            get
            {
                return _repoStatistic;
            }
        }

        protected static IMapper Mapper { get; set; }

        protected static Action<SxBaseController<TDbContext>> WriteBreadcrumbs { get; set; }

        public SxBaseController()
        {
            if (Mapper == null)
                Mapper = SxApplication<TDbContext>.MapperConfiguration.CreateMapper();
            if (_repoAffiliateLink == null)
                _repoAffiliateLink = new SxRepoAffiliateLink<TDbContext>();
            if (_repoRequest == null)
                _repoRequest = new SxRepoRequest<TDbContext>();
            if (_repoStatistic == null)
                _repoStatistic = new SxRepoStatistic<TDbContext>();
        }

        public string SxAreaName { get; set; }
        public string SxControllerName { get; set; }
        public string SxActionName { get; set; }
        public string SxRawUrl { get; set; }
        public Uri SxUrlReferrer { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeDataValues = filterContext.RequestContext.RouteData.Values;
            SxAreaName = routeDataValues["area"]?.ToString().ToLower();
            SxControllerName = routeDataValues["controller"].ToString().ToLower();
            SxActionName = routeDataValues["action"].ToString().ToLower();
            SxRawUrl = Request.RawUrl.ToLower();
            SxUrlReferrer = Request.UrlReferrer;

            //если экшн является дочерним или задан аттрибут нелогирования запроса
            var notLogRequest = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(NotLogRequestAttribute)) != null;
            if (filterContext.IsChildAction || notLogRequest) return;

            //забаненные адреса
            if (SxUrlReferrer != null)
            {
                if (SxApplication<TDbContext>.GetBannedUrls().Contains(SxUrlReferrer.ToString()))
                {
                    filterContext.Result = new HttpStatusCodeResult(403);
                    return;
                }
            }

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

            //пишем хлебные крошки
            if (WriteBreadcrumbs != null)
                WriteBreadcrumbs(this);

            //пишем идентификационный куки
            var identityCookie = checkIdentityCookie(Request, Response);

            //пишем куки партнерок
            writeAffiliateCookies();

            //пишем информацию о запросе
            if (Equals(SxApplication<TDbContext>.LoggingRequest, true) && SxAreaName != "admin")
            {
                writeRequestInfo(identityCookie);
            }

            base.OnActionExecuting(filterContext);
        }

        private Sx301Redirect get301Redirect(CacheItemPolicy cip = null)
        {
            cip = cip ?? Managers.SxCacheExpirationManager.GetExpiration(minutes: 60);
            var redirect = (Sx301Redirect)SxApplication<TDbContext>.AppCache["CACHE_REDIRECT_" + SxRawUrl];
            if (redirect == null)
            {
                redirect = new SxRepo301Redirect<TDbContext>().Get301Redirect(SxRawUrl);
                SxApplication<TDbContext>.AppCache.Add("CACHE_REDIRECT_" + SxRawUrl, redirect, cip);
            }

            return redirect;
        }

        private SxSeoTags getPageSeoTags(CacheItemPolicy cip = null)
        {
            var seoTags = (SxSeoTags)SxApplication<TDbContext>.AppCache["CACHE_SEOTAGS_" + SxRawUrl];
            if (seoTags == null)
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
            if (seoTags.Keywords.Any())
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

        private void writeRequestInfo(string identityCookie)
        {
            Task.Run(() =>
            {
                var requestInstance = new SxRequest
                {
                    Browser = Request.Browser != null ? Request.Browser.Browser : null,
                    ClientIP = Request.ServerVariables["REMOTE_ADDR"],
                    RawUrl = Request.RawUrl.ToLowerInvariant(),
                    RequestType = Request.RequestType,
                    UrlRef = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri.ToLower() : null,
                    SessionId = identityCookie,
                    UserAgent = Request.UserAgent
                };
                _repoRequest.Create(requestInstance);
            });
        }

        private void writePageBanners()
        {
            var rawUrl = Request.RawUrl;
            ViewBag.PageBanners = SxApplication<TDbContext>.BannerProvider.GetPageBanners(rawUrl);
        }

        private static readonly string _affiliateCookieName = ConfigurationManager.AppSettings["AffiliateCookieName"];
        private static readonly string _affiliateQueryParName = ConfigurationManager.AppSettings["AffiliateQueryParName"];
        private void writeAffiliateCookies()
        {
            var ak = Request.QueryString.Get(_affiliateQueryParName);
            var afc = Request.Cookies.Get(_affiliateCookieName);
            if (afc == null && string.IsNullOrEmpty(ak)) return;

            var cookies = Request.Cookies[_affiliateCookieName];

            Guid akGuid;
            if (!Guid.TryParse(ak, out akGuid)) return;

            if (!string.IsNullOrEmpty(ak))
                _repoAffiliateLink.AddViewAsync(akGuid);

            if (cookies == null && !string.IsNullOrEmpty(ak))
            {
                var list = new List<string>() { ak };
                Response.Cookies.Add(getAffiliateCookie(list));
            }
            else if (cookies != null && !string.IsNullOrEmpty(ak))
            {
                var list = JsonConvert.DeserializeObject<List<string>>(cookies.Value);
                if (!list.Contains(ak))
                {
                    list.Add(ak);
                    Response.Cookies.Remove(_affiliateCookieName);
                    Response.Cookies.Add(getAffiliateCookie(list));
                }
            }
        }
        private static HttpCookie getAffiliateCookie(List<string> list)
        {
            return new HttpCookie(_affiliateCookieName, JsonConvert.SerializeObject(list));
        }

        private static readonly string _identityCookieName = "sx_id";
        private static string checkIdentityCookie(HttpRequestBase request, HttpResponseBase response)
        {
            var identityCookie = request.Cookies[_identityCookieName];
            if (identityCookie != null) return identityCookie.Value;

            var guid = Guid.NewGuid().ToString().ToLower();
            identityCookie = new HttpCookie(_identityCookieName, guid);
            response.Cookies.Add(identityCookie);
            return identityCookie.Value;
        }

        protected string IdentityCookieValue
        {
            get
            {
                var identityCookie = Request.Cookies[_identityCookieName];
                return identityCookie?.Value;
            }
        }
    }
}
