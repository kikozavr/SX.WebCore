using AutoMapper;
using Newtonsoft.Json;
using SX.WebCore.Attrubutes;
using SX.WebCore.Managers;
using SX.WebCore.MvcApplication;
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

namespace SX.WebCore.MvcControllers.Abstract
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class SxBaseController : Controller
    {
        protected static IMapper Mapper { get; set; }

        protected static Action<SxBaseController> WriteBreadcrumbs { get; set; }

        public SxBaseController()
        {
            if (Mapper == null)
                Mapper = SxMvcApplication.MapperConfiguration.CreateMapper();
        }

        public string SxAreaName { get; set; }
        public string SxControllerName { get; set; }
        public string SxActionName { get; set; }
        public string SxRawUrl { get; set; }
        public Uri SxUrlReferrer { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeDataValues = filterContext.RequestContext.RouteData.Values;
            var area = filterContext.RequestContext.RouteData.DataTokens["area"] ?? routeDataValues["area"];
            SxAreaName = area?.ToString().ToLower();
            SxControllerName = routeDataValues["controller"].ToString().ToLower();
            SxActionName = routeDataValues["action"].ToString().ToLower();
            SxRawUrl = Request.RawUrl.ToLower();
            SxUrlReferrer = Request.UrlReferrer;

            clearResponseHeaders(filterContext.HttpContext.Response);

            if (SxAreaName == "admin") return;

            //если экшн является дочерним или задан аттрибут нелогирования запроса
            var notLogRequest = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(NotLogRequestAttribute)) != null;
            if (filterContext.IsChildAction || notLogRequest) return;

            //забаненные адреса
            if (SxUrlReferrer != null)
            {
                if (SxMvcApplication.GetBannedUrls().Contains(SxUrlReferrer.ToString()))
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

            //пишем информацию о запросе (не для админ area)
            writeRequestInfo(identityCookie);

            base.OnActionExecuting(filterContext);
        }
        private static void clearResponseHeaders(HttpResponseBase response)
        {
            response.Headers.Remove("Server");
        }

        private SxRedirect get301Redirect()
        {
            var key = "CACHE_REDIRECT_" + SxRawUrl;
            var redirect = SxMvcApplication.CacheProvider.Get<SxRedirect>(key);
            if (redirect == null)
            {
                redirect = SxRedirectsController.Repo.GetPageRedirect(SxRawUrl);
                SxMvcApplication.CacheProvider.Set(key, redirect ?? new SxRedirect { OldUrl = SxRawUrl, NewUrl = null }, 60);
            }

            return redirect;
        }

        private SxSeoTags getPageSeoTags()
        {
            var key = "CACHE_SEOTAGS_" + SxRawUrl;
            var seoTags = SxMvcApplication.CacheProvider.Get<SxSeoTags>(key);
            if (seoTags == null)
            {
                seoTags = SxSeoTagsController.Repo.GetSeoTags(SxRawUrl);
                SxMvcApplication.CacheProvider.Set(key, seoTags ?? new SxSeoTags(), 60);
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
            if (Request.UserAgent == null) return;

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
                SxRequestsController.Repo.Create(requestInstance);
            });
        }

        private void writePageBanners()
        {
            var rawUrl = Request.RawUrl;
            ViewBag.PageBanners = SxMvcApplication.BannerProvider.GetPageBanners(rawUrl);
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
                SxAffiliateLinksController.Repo.AddViewAsync(akGuid);

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
            identityCookie = new HttpCookie(_identityCookieName, guid) { Expires=DateTime.Now.AddYears(1)};
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

        [NonAction]
        protected static async Task SendNoReplyMailAsync(string body, string[] mailsTo, string subject)
        {
            var smtpUserName = ConfigurationManager.AppSettings["NoReplyEmail"];
            var smtpUserPassword = ConfigurationManager.AppSettings["NoReplyEmailPassword"];
            var smtpHost = ConfigurationManager.AppSettings["NoReplySmtpHost"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["NoReplySmtpPort"]);

            var mailManager = new SxAppMailManager(smtpUserName, smtpUserPassword, smtpHost, smtpPort);
            await mailManager.SendMail(
                mailFrom: smtpUserName,
                mailsTo: mailsTo,
                subject: subject,
                body: body,
                isBodyHtml: true
                );
        }
    }
}
