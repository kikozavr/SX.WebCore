﻿using Newtonsoft.Json;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxMaterialsController<TModel, TDbContext> : SxBaseController<TDbContext>
        where TModel : SxMaterial
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

        private static SxRepoSeoTags<TDbContext> _repoSeoTags;
        protected SxMaterialsController(ModelCoreType mct)
        {
            _mct = mct;
            if (_repoSeoTags == null)
                _repoSeoTags = new SxRepoSeoTags<TDbContext>();
        }

        private static SxRepoMaterial<TModel, TDbContext> _repo;
        protected static SxRepoMaterial<TModel, TDbContext> Repo
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

        [HttpGet]
#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public async Task<JsonResult> DateStatistic()
        {
            return await Task.Run(() =>
            {
                var data = (Repo as SxRepoMaterial<TModel, TDbContext>).DateStatistic;
                return Json(data, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpGet, AllowAnonymous]
        public virtual ViewResult Add()
        {
            var viewModel = default(TModel);
            return View(viewModel);
        }

        [HttpGet, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(TModel model)
        {
            if (await _repo.GetByKeyAsync(model.Id, model.ModelCoreType)==null)
                return new HttpNotFoundResult();

            _repoSeoTags.DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
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
    }
}
