using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxMaterialsController<TModel, TDbContext> : SxBaseController<TDbContext>
        where TModel : SxDbModel<int>
        where TDbContext : SxDbContext
    {
        private ModelCoreType _mct;
        public SxMaterialsController(ModelCoreType mct)
        {
            _mct = mct;
        }

        private static SxDbRepository<int, TModel, TDbContext> _repo;
        protected static SxDbRepository<int, TModel, TDbContext> Repo
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


        [HttpGet]
#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        public async Task<JsonResult> DateStatistic()
        {
            return await Task.Run(() =>
            {
                var data = (Repo as SxRepoMaterial<TModel, TDbContext>).DateStatistic(_mct);
                return Json(data, JsonRequestBehavior.AllowGet);
            });
        }
    }
}
