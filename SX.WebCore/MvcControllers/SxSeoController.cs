using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using SX.WebCore.ViewModels;
using System.Web.Mvc;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    public abstract class SxSeoController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        [HttpGet]
        public virtual ViewResult EditRobotsFile()
        {
            var settings = new SxRepoSiteSetting<TDbContext>().GetByKeys(
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
                        new SxRepoSiteSetting<TDbContext>().Create(settings[i]);
                    }

                    TempData["EditEmptyGameMessage"] = "Настройки успешно сохранены";
                    return (ActionResult)RedirectToAction("editrobotsfile");
                }
                else if (isExists && isModified)
                {
                    new SxRepoSiteSetting<TDbContext>().Update(new SxSiteSetting { Id = Settings.robotsFileSetting, Value = model.FileContent }, true, "Value");
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
    }
}
