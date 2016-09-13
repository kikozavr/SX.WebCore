using SX.WebCore.HtmlHelpers;
using SX.WebCore.Managers;
using SX.WebCore.MvcApplication;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SX.WebCore.MvcControllers
{
    public class SxValutesController<TDbContext> : SxBaseController<TDbContext> where TDbContext : SxDbContext
    {
        private static readonly int _pageSize = 15;

        [HttpGet]
        public virtual ViewResult Index(int page = 1, DateTime? date = null)
        {
            var data = getValutes(date);
            var viewModel = data.Skip(_pageSize * (page - 1)).Take(_pageSize).ToArray();

            var order = new SxOrder { FieldName = "Name", Direction = SxExtantions.SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };
            filter.PagerInfo.TotalItems = data.Length;
            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMValute filterModel, SxOrder order, int page = 1)
        {
            var data = getValutes(null, filterModel, order);
            page = data.Length <= _pageSize ? 1 : page;
            var viewModel = data.Skip(_pageSize * (page - 1)).Take(_pageSize).ToArray();

            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = data.Length;
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        private static SxVMValute[] getValutes(DateTime? date = null, SxVMValute filterModel = null, SxOrder order = null)
        {
            var d = date == null ? DateTime.Now : (DateTime)date;
            var strD = d.ToString("dd/MM/yyyy");
            var url = string.Format("http://www.cbr.ru/scripts/XML_daily.asp?date_req={0}", strD);

            if (SxMvcApplication<TDbContext>.AppCache.Get("CACHE_VALUTES") == null)
                SxMvcApplication<TDbContext>.AppCache.Add(new CacheItem("CACHE_VALUTES", XDocument.Load(url)), SxCacheExpirationManager.GetExpiration(minutes:120));
            var doc = (XDocument)SxMvcApplication<TDbContext>.AppCache.Get("CACHE_VALUTES");

            var data = doc.Descendants("Valute")
                .Select(x => new SxVMValute
                {
                    Id = x.Attribute("ID").Value,
                    NumCode = Convert.ToInt16(x.Element("NumCode").Value),
                    CharCode = x.Element("CharCode").Value,
                    Nominal = Convert.ToDecimal(x.Element("Nominal").Value),
                    Name = x.Element("Name").Value,
                    Value = Convert.ToDecimal(x.Element("Value").Value)
                }).ToArray();

            if (filterModel != null)
            {
                if (filterModel.Id != null)
                    data = data.Where(x => x.Id.Contains(filterModel.Id)).ToArray();
                if (filterModel.NumCode != 0)
                    data = data.Where(x => x.NumCode >= filterModel.NumCode).ToArray();
                if (filterModel.CharCode != null)
                    data = data.Where(x => x.CharCode.Contains(filterModel.CharCode)).ToArray();
                if (filterModel.Nominal != 0)
                    data = data.Where(x => x.Nominal >= filterModel.Nominal).ToArray();
                if (filterModel.Name != null)
                    data = data.Where(x => x.Name.ToUpper().Contains(filterModel.Name.ToUpper())).ToArray();
                if (filterModel.Value != 0)
                    data = data.Where(x => x.Value >= filterModel.Value).ToArray();
            }

            if (order != null)
            {
                if (order.FieldName == "Id")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Id).ToArray() : data.OrderByDescending(x => x.Id).ToArray();
                if (order.FieldName == "NumCode")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.NumCode).ToArray() : data.OrderByDescending(x => x.NumCode).ToArray();
                if (order.FieldName == "CharCode")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.CharCode).ToArray() : data.OrderByDescending(x => x.CharCode).ToArray();
                if (order.FieldName == "Nominal")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Nominal).ToArray() : data.OrderByDescending(x => x.Nominal).ToArray();
                if (order.FieldName == "Name")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Name).ToArray() : data.OrderByDescending(x => x.Name).ToArray();
                if (order.FieldName == "Value")
                    data = order.Direction == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Value).ToArray() : data.OrderByDescending(x => x.Value).ToArray();
            }

            return data;
        }

        [HttpPost]
        public JsonResult GetCurCourse(string cc)
        {
            var strD = DateTime.Now.ToString("dd/MM/yyyy");
            var url = string.Format("http://www.cbr.ru/scripts/XML_daily.asp?date_req={0}", strD);

            var doc = (XDocument)SxMvcApplication<TDbContext>.AppCache["CACHE_VALUTES_XML_DOCUMENT"];
            if (doc == null)
            {
                doc = XDocument.Load(url);
                SxMvcApplication<TDbContext>.AppCache.Add("CACHE_VALUTES_XML_DOCUMENT", doc, SxCacheExpirationManager.GetExpiration(minutes: 60));
            }

            var data = doc.Descendants("Valute")
                .Select(x => new SxVMValute
                {
                    Id = x.Attribute("ID").Value,
                    NumCode = Convert.ToInt16(x.Element("NumCode").Value),
                    CharCode = x.Element("CharCode").Value,
                    Nominal = Convert.ToDecimal(x.Element("Nominal").Value),
                    Name = x.Element("Name").Value,
                    Value = Convert.ToDecimal(x.Element("Value").Value)
                }).SingleOrDefault(x => x.CharCode == cc);

            return Json(data);
        }
    }
}
