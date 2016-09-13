using SX.WebCore.MvcApplication;
using SX.WebCore.Providers;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public class SxSeoController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        public static SxRepoSiteSetting<TDbContext> _repo = new SxRepoSiteSetting<TDbContext>();
        public static SxRepoSiteSetting<TDbContext> Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        protected static Func<dynamic, string> SeoItemUrlFunc { get; set; }

        [HttpGet]
        public virtual ViewResult EditRobotsFile()
        {
            var settings = _repo.GetByKey(Settings.robotsFileSetting);

            var viewModel = new SxVMRobotsFile
            {
                FileContent = settings?.Value
            };
            viewModel.OldFileContent = viewModel.FileContent;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult EditRobotsFile(SxVMRobotsFile model)
        {
            if (ModelState.IsValid)
            {
                var isExists = !string.IsNullOrEmpty(model.OldFileContent);
                var isModified = !Equals(model.FileContent, model.OldFileContent);

                if (!isExists)
                {
                    var settings = new SxSiteSetting[] {
                        new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }
                    };
                    for (int i = 0; i < settings.Length; i++)
                    {
                        _repo.Create(settings[i]);
                    }

                    ViewBag.EditSiteSettingsMessage = "Настройки успешно сохранены";
                    return RedirectToAction("EditRobotsFile", "Seo");
                }
                else if (isExists && isModified)
                {
                    var data = _repo.Update(new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }, true, "Value");
                    ViewBag.EditSiteSettingsMessage = "Настройки успешно обновлены";
                    model = new SxVMRobotsFile
                    {
                        FileContent = data.Value,
                        OldFileContent = data.Value
                    };
                    return View(model);
                }
                else
                {
                    ViewBag.EditSiteSettingsMessage = "В настройках нет изменений";
                    return View(model);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [OutputCache(Duration = 900)]
        public virtual ContentResult Robotstxt()
        {
            var fileContent = SxMvcApplication<TDbContext>.SiteSettingsProvider.Get(Settings.robotsFileSetting);
            if (fileContent != null)
                return Content(fileContent.Value, "text/plain", Encoding.UTF8);
            else return null;
        }

#if !DEBUG
        [OutputCache(Duration = 86400)]
#endif
        [AllowAnonymous]
        public async Task<ContentResult> Sitemap()
        {
            var provider = new SxSiteMapProvider();
            var data = await Repo.GetSiteMapAsync();
            var viewModel = new SxVMSiteMapUrl[0];
            if (data.Any())
            {
                SxVMMaterial item = null;
                viewModel = new SxVMSiteMapUrl[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    item = data[i];
                    viewModel[i] = new SxVMSiteMapUrl(Url.ContentAbsUrl(SeoItemUrlFunc != null ? SeoItemUrlFunc(item) : null)) {
                        LasMod = item.DateUpdate
                    };
                }
            }

            return Content(provider.GenerateSiteMap(ref viewModel), "text/xml");
        }
    }
}
