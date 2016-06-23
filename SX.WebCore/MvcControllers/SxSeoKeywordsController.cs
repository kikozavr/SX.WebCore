using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Linq;
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
        public virtual PartialViewResult Index(int sid, int page = 1)
        {
            var order = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { WhereExpressionObject = new SxSeoKeyword { SeoInfoId = sid }, Order=order };
            filter.PagerInfo.TotalItems = _repo.Count(filter);

            var viewModel = _repo.Query(filter)
                .Select(x=>Mapper.Map<SxSeoKeyword, SxVMSeoKeyword>(x))
                .ToArray();

            ViewBag.Filter = filter;
            ViewBag.SeoInfoId = sid;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int sid, SxVMSeoKeyword filterModel, SxOrder order, int page = 1)
        {
            filterModel.SeoInfoId = sid;
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = _repo.Count(filter);
            filter.PagerInfo.Page = filter.PagerInfo.TotalItems <= _pageSize ? 1 : page;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter)
                .Select(x => Mapper.Map<SxSeoKeyword, SxVMSeoKeyword>(x))
                .ToArray();

            ViewBag.Filter = filter;
            ViewBag.SeoInfoId = sid;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Edit(SxVMEditSeoKeyword model)
        {
            var redactModel = Mapper.Map<SxVMEditSeoKeyword, SxSeoKeyword>(model);
            if (ModelState.IsValid)
            {
                SxSeoKeyword newModel = null;
                if (model.Id == 0)
                {
                    var exist = _repo.All.FirstOrDefault(x => x.SeoInfoId == model.SeoInfoId && x.Value == model.Value) != null;
                    if (!exist)
                        newModel = _repo.Create(redactModel);
                }
                else
                    newModel = _repo.Update(redactModel, true, "SeoInfoId", "Value");
            }

            return RedirectToAction("index", new { controller="seokeywords", sid = model.SeoInfoId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(SxVMEditSeoKeyword model)
        {
            var seoInfoId = model.SeoInfoId;
            _repo.Delete(model.Id);
            return RedirectToAction("index", new { controller = "seokeywords", sid = model.SeoInfoId });
        }
    }
}
