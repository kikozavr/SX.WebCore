﻿using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcControllers.Abstract;
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
    public abstract class SxMaterialsController<TModel, TViewModel> : SxBaseController
        where TModel : SxMaterial
        where TViewModel : SxVMMaterial, new()
    {
        private static ModelCoreType _mct;
        protected static ModelCoreType ModelCoreType
        {
            get
            {
                return _mct;
            }
        }

        private static SxRepoMaterial<TModel, TViewModel> _repo;
        public static SxRepoMaterial<TModel, TViewModel> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        protected SxMaterialsController(ModelCoreType mct)
        {
            _mct = mct;
        }

        private static readonly int _pageSize = 10;
        [HttpGet]
        public virtual ActionResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View("_IndexMaterials", viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Index(TViewModel filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridMaterials", viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int? id = null)
        {
            var viewModel = id.HasValue
                ? Mapper.Map<TModel, TViewModel>(_repo.GetByKey(id))
                : new TViewModel();
            if (id.HasValue && viewModel == null) return new HttpNotFoundResult();

            if (viewModel.Category != null)
                ViewBag.MaterialCategoryTitle = viewModel.Category.Title;
            if (viewModel.FrontPicture != null)
                ViewData["FrontPictureIdCaption"] = viewModel.FrontPicture.Caption;

            ViewBag.ModelCoreType = _mct;

            return View("_EditMaterial", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TViewModel model)
        {
            model.ModelCoreType = _mct;
            model.UserId = User.Identity.GetUserId();

            var isNew = model.Id == 0;
            if (isNew || (!isNew && string.IsNullOrEmpty(model.TitleUrl)))
                model.TitleUrl = Url.SeoFriendlyUrl(model.Title);

            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<TViewModel, TModel>(model);
                
                if (isNew)
                    _repo.Create(redactModel);
                else
                    _repo.Update(redactModel);

                return RedirectToAction("Index", "Articles");
            }
            else
            {
                ViewBag.ModelCoreType = _mct;
                return View(model);
            }
        }

        protected Func<SxFilter, bool> BeforeSelectListAction { get; set; }
        [HttpGet]
        public virtual ActionResult List(SxFilter filter)
        {
            var routeDataValues = Request.RequestContext.RouteData.Values;
            var page = routeDataValues["page"] != null ? Convert.ToInt32(routeDataValues["page"]) : 1;
            filter.PagerInfo.Page = page;
            filter.PagerInfo.PageSize = 5;

            if (BeforeSelectListAction != null && !BeforeSelectListAction(filter))
                return new HttpNotFoundResult();

            var viewModel = new SxPagedCollection<TViewModel>();

            //material category
            var cat = Request.QueryString.Get("cat");
            if(!string.IsNullOrEmpty(cat))
            {
                filter.CategoryId = cat;
                var category = SxMaterialCategoriesController<SxVMMaterialCategory>.Repo.GetByKey(cat);
                ViewBag.Category = Mapper.Map<SxMaterialCategory, SxVMMaterialCategory>(category);
            }

            //material tag
            var tag = Request.QueryString.Get("tag");
            if (!string.IsNullOrEmpty(tag))
            {
                filter.Tag = SxMaterialTagsController.Repo.GetByKey(tag, _mct);
                ViewBag.Tag = Mapper.Map<SxMaterialTag, SxVMMaterialTag>(filter.Tag);
            }

            viewModel.Collection = Repo.Read(filter);
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
            if (await _repo.GetByKeyAsync(model.Id, model.ModelCoreType) == null)
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
            if (existsMaterialByTitleUrl)
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

        [ChildActionOnly]
        public virtual PartialViewResult LikeMaterials(SxFilter filter, int amount = 10)
        {
            var viewModel = _repo.GetLikeMaterials(filter, amount);
            ViewBag.ModelCoreType = filter.ModelCoreType;

            return PartialView("_LikeMaterials", viewModel);
        }

        [ChildActionOnly]
        public virtual PartialViewResult ByDateMaterial(int mid, ModelCoreType mct, bool dir = false, int amount = 3)
        {
            var viewModel = _repo.GetByDateMaterials(mid, mct, dir, amount);
            ViewBag.ModelCoreType = mct;
            return PartialView("_ByDateMaterial", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;mid;amount")]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult Popular(int? mid = null, int amount = 4)
        {
            var viewModel = Repo.GetPopular(_mct, mid, amount);
            ViewData["ModelCoreType"] = _mct;

            return PartialView("_PopularMaterials", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;amount")]
#endif
        public virtual PartialViewResult Last(ModelCoreType? mct = null, int amount = 5, int? mid = null)
        {
            var viewModel = Repo.Last(mct, amount);
            return PartialView("_Last", viewModel);
        }
    }
}
