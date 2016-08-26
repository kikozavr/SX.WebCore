using Newtonsoft.Json;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxMaterialsController<TModel, TViewModel, TDbContext> : SxBaseController<TDbContext>
        where TModel : SxMaterial
        where TViewModel : SxVMMaterial
        where TDbContext : SxDbContext
    {
        private static ModelCoreType _mct;
        protected static ModelCoreType ModelCoreType
        {
            get
            {
                return _mct;
            }
        }

        private static SxRepoMaterial<TModel, TViewModel, TDbContext> _repo;
        private static SxRepoSeoTags<TDbContext> _repoSeoTags;
        protected SxMaterialsController(ModelCoreType mct)
        {
            _mct = mct;
            if (_repoSeoTags == null)
                _repoSeoTags = new SxRepoSeoTags<TDbContext>();
        }

        protected static SxRepoMaterial<TModel, TViewModel, TDbContext> Repo
        {
            get
            {
                return _repo;
            }
            set
            {
                _repo = value;
            }
        }
        protected static SxRepoSeoTags<TDbContext> RepoSeoTags
        {
            get
            {
                return _repoSeoTags;
            }
        }

        protected Func<SxFilter, bool> BeforeSelectListAction { get; set; }
        [HttpGet]
        public virtual ActionResult List(SxFilter filter)
        {
            var routeDataValues = Request.RequestContext.RouteData.Values;
            var page = routeDataValues["page"] != null ? Convert.ToInt32(routeDataValues["page"]) : 1;
            filter.PagerInfo.Page = page;
            filter.PagerInfo.PageSize = 10;

            if (BeforeSelectListAction != null && !BeforeSelectListAction(filter))
                return new HttpNotFoundResult();

            var viewModel = new SxPagedCollection<TViewModel>();
            var tag = Request.QueryString.Get("tag");
            if (!string.IsNullOrEmpty(tag))
            {
                filter.Tag = tag;
                ViewBag.Tag = tag;
            }

            viewModel.Collection = Repo.Read(filter).Select(x=>Mapper.Map<TModel, TViewModel>(x)).ToArray();
            viewModel.PagerInfo = new SxPagerInfo(filter.PagerInfo.Page, filter.PagerInfo.PageSize)
            {
                PagerSize = 3,
                TotalItems = filter.PagerInfo.TotalItems
            };

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(TModel model)
        {
            if (await _repo.GetByKeyAsync(model.Id, model.ModelCoreType)==null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet, AllowAnonymous]
        public virtual ViewResult Add()
        {
            var viewModel = default(TModel);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async virtual Task<ActionResult> Add(TModel model)
        {
            var secretKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSecretKey"];
            var trueCaptcha = await validateCaptcha(ModelState, secretKey, Request["g-recaptcha-response"]);
            if (!trueCaptcha)
            {
                ModelState.AddModelError("UserName", "Не пройдена проверка Recaptcha");
                return View(model);
            }

            var type = model.GetType();
            var titleUrl = type.GetProperty("Title").GetValue(model).ToString();
            var titleUrlProp = type.GetProperty("TitleUrl");
            titleUrlProp.SetValue(model, Url.SeoFriendlyUrl(titleUrl));

            var existsMaterialByTitleUrl = _repo.ExistsMaterialByTitleUrl(titleUrl);
            if(existsMaterialByTitleUrl)
            {
                ModelState.AddModelError("Title", "Материал с таким названием уже существует в БД. Выберите другое название");
                return View(model);
            }

            _repo.Create(model);
            TempData["SuccessMessage"] = "ok";
            return RedirectToAction("Add");
        }
        private async Task<bool> validateCaptcha(ModelStateDictionary modelState, string secretKey, string response)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(response)) return false;

                using (var client = new WebClient())
                {
                    var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
                    return captchaResponse.Success == "true";
                }
            });
        }
        private class CaptchaResponse
        {
            [JsonProperty("success")]
            public string Success { get; set; }
        }

        [HttpGet]
#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public async Task<JsonResult> DateStatistic()
        {
            return await Task.Run(() =>
            {
                var data = Repo.DateStatistic;
                return Json(data, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddLike(int mid, bool ld)
        {
            var data = await _repo.AddLikeAsync(ld, mid, _mct);
            return Json(data);
        }

        [HttpGet, ChildActionOnly]
        public virtual PartialViewResult LikeMaterials(SxFilter filter, int amount = 10)
        {
            var viewModel = _repo.GetLikeMaterial(filter, amount);
            ViewBag.ModelCoreType = filter.ModelCoreType;

            return PartialView("~/Views/Shared/_LikeMaterial.cshtml", viewModel);
        }

        [HttpGet, NotLogRequest]
        public virtual PartialViewResult ByDateMaterial(int mid, ModelCoreType mct, bool dir = false, int amount = 3)
        {
            var viewModel = _repo.GetByDateMaterials(mid, mct, dir, amount);
            ViewBag.ModelCoreType = mct;
            return PartialView("~/Views/Shared/_ByDateMaterial.cshtml", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;mid;amount")]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult Popular(int? mid = null, int amount = 4)
        {
            var viewModel = Repo.GetPopular(_mct, mid, amount);
            ViewData["ModelCoreType"] = _mct;

            return PartialView("~/Views/Shared/_PopularMaterials.cshtml", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;amount")]
#endif
        public virtual PartialViewResult Last(ModelCoreType? mct=null, int amount=5)
        {
            var viewModel = Repo.Last(mct, amount);
            return PartialView("_Last", viewModel);
        }
    }
}
