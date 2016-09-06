﻿using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using SX.WebCore.ViewModels;
using System;
using System.Text;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "admin")]
    public abstract class SxSiteSettingsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private const string __notSetSettingValue = "Настройка не определена";
        private static SxRepoSiteSetting<TDbContext> _repo;
        private static SxRepoPicture<TDbContext> _repoPicture;
        public SxSiteSettingsController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteSetting<TDbContext>();
            if (_repoPicture == null)
                _repoPicture = new SxRepoPicture<TDbContext>();
        }

        protected static SxRepoPicture<TDbContext> RepoPicture
        {
            get
            {
                return _repoPicture;
            }
        }

        [HttpGet, AllowAnonymous]
        public virtual ActionResult ReleaseNotes()
        {
            return View(model: Files.ReleaseNotes);
        }

        [HttpGet, Authorize(Roles = "admin")]
        public virtual ViewResult EditSite()
        {

            var settings = _repo.GetByKeys(
                    Settings.siteDomain,
                    Settings.siteLogoPath,
                    Settings.siteName,
                    Settings.siteBgPath,
                    Settings.siteFaveiconPath,
                    Settings.siteDesc
                );

            var viewModel = new SxVMSiteSettings
            {
                SiteDomain = settings.ContainsKey(Settings.siteDomain) ? settings[Settings.siteDomain].Value : null,
                LogoPath = settings.ContainsKey(Settings.siteLogoPath) ? settings[Settings.siteLogoPath].Value : null,
                SiteName = settings.ContainsKey(Settings.siteName) ? settings[Settings.siteName].Value : null,
                SiteBgPath = settings.ContainsKey(Settings.siteBgPath) ? settings[Settings.siteBgPath].Value : null,
                SiteFaveiconPath = settings.ContainsKey(Settings.siteFaveiconPath) ? settings[Settings.siteFaveiconPath].Value : null,
                SiteDesc = settings.ContainsKey(Settings.siteDesc) ? settings[Settings.siteDesc].Value : null,
            };

            viewModel.OldSiteDomain = viewModel.SiteDomain;
            viewModel.OldLogoPath = viewModel.LogoPath;
            viewModel.OldSiteName = viewModel.SiteName;
            viewModel.OldSiteBgPath = viewModel.SiteBgPath;
            viewModel.OldSiteFaveiconPath = viewModel.SiteFaveiconPath;
            viewModel.OldSiteDesc = viewModel.SiteDesc;

            Guid guid;

            if (settings.ContainsKey(Settings.siteLogoPath))
            {
                Guid.TryParse(settings[Settings.siteLogoPath].ToString(), out guid);
                if (guid != Guid.Empty)
                    ViewData["LogoPathCaption"] = _repoPicture.GetByKey(guid).Caption;
            }

            if (settings.ContainsKey(Settings.siteBgPath))
            {
                Guid.TryParse(settings[Settings.siteBgPath].ToString(), out guid);
                if (guid != Guid.Empty)
                    ViewData["SiteBgPathCaption"] = _repoPicture.GetByKey(guid).Caption;
            }


            if (settings.ContainsKey(Settings.siteFaveiconPath))
            {
                Guid.TryParse(settings[Settings.siteFaveiconPath].ToString(), out guid);
                if (guid != Guid.Empty)
                    ViewData["SiteFaveiconPathCaption"] = _repoPicture.GetByKey(guid).Caption;
            }

            return View(viewModel);
        }

        [Authorize(Roles = "admin"), HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult EditSite(SxVMSiteSettings model)
        {
            if (ModelState.IsValid)
            {
                var isExists = !string.IsNullOrEmpty(model.OldSiteDomain) || !string.IsNullOrEmpty(model.OldLogoPath) || !string.IsNullOrEmpty(model.OldSiteName) || !string.IsNullOrEmpty(model.OldSiteBgPath) || !string.IsNullOrEmpty(model.OldSiteFaveiconPath) || !string.IsNullOrEmpty(model.OldSiteDesc);
                var isModified = !Equals(model.SiteDomain, model.OldSiteDomain) || !Equals(model.LogoPath, model.OldLogoPath) || !Equals(model.SiteName, model.OldSiteName) || !Equals(model.SiteBgPath, model.OldSiteBgPath) || !Equals(model.SiteFaveiconPath, model.OldSiteFaveiconPath) || !Equals(model.SiteDesc, model.OldSiteDesc);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.siteDomain, Value = model.SiteDomain },
                        new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath },
                        new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName },
                        new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath },
                        new SxSiteSetting { Id = Settings.siteFaveiconPath, Value = model.SiteFaveiconPath },
                        new SxSiteSetting { Id = Settings.siteDesc, Value = model.SiteDesc }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        var setting = settings[i];
                        _repo.Create(setting);
                    }

                    ViewBag.EditSiteSettingsMessage = "Настройки успешно сохранены";
                    SxApplication<TDbContext>.SiteSettingsProvider.Set(Settings.siteDomain, model.SiteDomain);
                    return RedirectToAction("editsite");
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.siteDomain, Value = model.SiteDomain }, true, "Value");

                    _repo.Update(new SxSiteSetting { Id = Settings.siteLogoPath, Value = model.LogoPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteName, Value = model.SiteName }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteBgPath, Value = model.SiteBgPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteFaveiconPath, Value = model.SiteFaveiconPath }, true, "Value");
                    _repo.Update(new SxSiteSetting { Id = Settings.siteDesc, Value = model.SiteDesc }, true, "Value");
                    ViewBag.EditSiteSettingsMessage = "Настройки успешно обновлены";
                    SxApplication<TDbContext>.SiteSettingsProvider.Set(Settings.siteDomain, model.SiteDomain);
                    return RedirectToAction("editsite");
                }
                else
                {
                    ViewBag.EditSiteSettingsMessage = "В настройках нет изменений";
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
