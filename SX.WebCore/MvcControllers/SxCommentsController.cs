using Microsoft.AspNet.Identity;
using SX.WebCore.Attrubutes;
using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxCommentsController : SxBaseController
    {
        private static SxRepoComment _repo = new SxRepoComment();
        public static SxRepoComment Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet, NotLogRequest]
        public PartialViewResult List(int mid, ModelCoreType mct, int page = 1)
        {
            var viewModel = getResult(mid, mct);

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            return PartialView("_List", viewModel);
        }

        [HttpGet]
        public PartialViewResult Edit(int mid, ModelCoreType mct)
        {
            var viewModel = new SxVMComment
            {
                MaterialId = mid,
                ModelCoreType = mct
            };

            ViewBag.NewCommentTitle = getNewCommentTitle(mct);

            return PartialView("_Edit", viewModel);
        }
        private static string getNewCommentTitle(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Article:
                    return "Комментировать статью";
                case ModelCoreType.News:
                    return "Комментировать новость";
                case ModelCoreType.Aphorism:
                    return "Комментировать афоризм";
                default:
                    return "Комментировать материал";
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public PartialViewResult Edit(SxVMComment model)
        {
            var isAuth = User.Identity.IsAuthenticated;
            if (isAuth)
            {
                ModelState["UserName"].Errors.Clear();
                ModelState["Email"].Errors.Clear();
                model.UserId = User.Identity.GetUserId();
            }
            if (ModelState.IsValid)
            {
                if (isAuth)
                    model.UserId = User.Identity.GetUserId();

                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<SxVMComment, SxComment>(model);
                if (isNew)
                    _repo.Create(redactModel);
            }

            var viewModel = getResult(model.MaterialId, model.ModelCoreType);
            return PartialView("_List", viewModel);
        }

        private SxVMComment[] getResult(int mid, ModelCoreType mct)
        {
            var filter = new SxFilter { MaterialId = mid, ModelCoreType = mct };
            var viewModel = _repo.Read(filter);
            return viewModel;
        }
    }
}
