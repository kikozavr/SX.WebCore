﻿using SX.WebCore.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxRatingController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoRating<TDbContext> _repo;
        public SxRatingController()
        {
            if (_repo == null)
                _repo = new SxRepoRating<TDbContext>();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> AddRating(SxRating model)
        {
            var data = await _repo.AddRatingAsync(model);
            return Json(data);
        }

        [ChildActionOnly]
        public virtual async Task<PartialViewResult> GetRating(int mid, ModelCoreType mct)
        {
            var data = await _repo.GetRatingAsync(mid, mct);
            return PartialView("_Rating");
        }
    }
}
