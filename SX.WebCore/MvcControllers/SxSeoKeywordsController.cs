using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.MvcControllers
{
    public abstract class SxSeoKeywordsController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static SxRepoSeoKeyword<TDbContext> _repo;
        public SxSeoKeywordsController()
        {
            if(_repo==null)
                _repo = new SxRepoSeoKeyword<TDbContext>();
        }

        private static readonly int _pageSize = 10;
        [HttpGet]
        public virtual PartialViewResult Index(int stid, int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order=order, AddintionalInfo=new object[] { stid } };

            var viewModel = _repo.Read(filter);

            ViewBag.Filter = filter;
            ViewBag.SeoTagsId = stid;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int stid, SxVMSeoKeyword filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, AddintionalInfo=new object[] { stid } };
            
            var viewModel = _repo.Read(filter);

            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;

            ViewBag.Filter = filter;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.SeoTagsId = stid;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Edit(SxVMSeoKeyword model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<SxVMSeoKeyword, SxSeoKeyword>(model);
                SxSeoKeyword newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "SeoTagsId", "Value");
            }

            return RedirectToAction("Index", "SeoKeywords", new { stid = model.SeoTagsId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(SxSeoKeyword model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            var seoTagsId = model.SeoTagsId;

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index", "SeoKeywords", new { stid = model.SeoTagsId });
        }
    }
}
