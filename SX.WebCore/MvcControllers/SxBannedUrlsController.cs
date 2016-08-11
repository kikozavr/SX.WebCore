using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles ="admin")]
    public abstract class SxBannedUrlsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static int _pageSize = 20;
        private static SxRepoBannedUrl<TDbContext> _repo;
        public SxBannedUrlsController()
        {
            if(_repo==null)
                _repo = new SxRepoBannedUrl<TDbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var order = new SxOrder { FieldName = "dbu.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter).Select(x=>Mapper.Map<SxBannedUrl, SxVMBannedUrl>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async virtual Task<PartialViewResult> Index(SxVMBannedUrl filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            
            var viewModel = (await _repo.ReadAsync(filter)).Select(x => Mapper.Map<SxBannedUrl, SxVMBannedUrl>(x)).ToArray();

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxBannedUrl();
            var seoInfo = Mapper.Map<SxBannedUrl, SxVMEditBannedUrl>(model);
            return View(seoInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SxVMEditBannedUrl model)
        {
            if (_repo.All.SingleOrDefault(x => x.Url == model.Url) != null)
                ModelState.AddModelError("Url", "Такая запись уже содержится в БД");

            var redactModel = Mapper.Map<SxVMEditBannedUrl, SxBannedUrl>(model);

            if (ModelState.IsValid)
            {
                SxBannedUrl newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Url", "Couse");

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxBannedUrl model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }
    }
}
