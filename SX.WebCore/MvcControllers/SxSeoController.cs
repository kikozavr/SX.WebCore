using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using SX.WebCore.ViewModels;
using System.Text;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxSeoController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        public static SxRepoSiteSetting<TDbContext> _repo;
        public SxSeoController()
        {
            if (_repo == null)
                _repo = new SxRepoSiteSetting<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult EditRobotsFile()
        {
            var settings = _repo.GetByKeys(
                                Settings.robotsFileSetting
                            );

            var viewModel = new SxVMRobotsFile
            {
                FileContent = settings.ContainsKey(Settings.robotsFileSetting) ? settings[Settings.robotsFileSetting].Value : null,
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

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return (ActionResult)RedirectToAction("editrobotsfile");
                }
                else if (isExists && isModified)
                {
                    _repo.Update(new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }, true, "Value");
                    TempData["EditEmptyGameMessage"] = "Настройки успешно обновлены";
                    return RedirectToAction("editrobotsfile");
                }
                else
                {
                    TempData["EditEmptyGameMessage"] = "В настройках нет изменений";
                    return View(model);
                }
            }

            return View(model);
        }

        [OutputCache(Duration = 900)]
        public virtual ContentResult Robotstxt()
        {
            var fileContent = SxApplication<TDbContext>.SiteSettingsProvider.Get(Settings.robotsFileSetting);
            if (fileContent != null)
                return Content(fileContent.Value, "text/plain", Encoding.UTF8);
            else return null;
        }
    }
}
