using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxPager(this HtmlHelper htmlHelper, SxPagerInfo pagerinfo, Func<int, string> pageUrl = null, bool isAjax = true, object htmlAttributes = null)
        {
            if (pagerinfo.TotalPages == 1) return null;

            var ul = new TagBuilder("ul");
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                ul.MergeAttributes(attributes, true);
            }
            ul.AddCssClass("sx-pager");

            var max = pagerinfo.CurrentPart * pagerinfo.PagerSize;
            var min = max - pagerinfo.PagerSize + 1;

            //first
            if (pagerinfo.CurrentPart > 1)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.First, isAjax, pageUrl);
            }

            //prev
            if (pagerinfo.CurrentPart > 1)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Prev, isAjax, pageUrl);
            }

            //normal
            for (int i = min; i <= max; i++)
            {
                if (i == pagerinfo.TotalPages + 1) break;

                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Normal, isAjax, pageUrl, i);
            }

            //next
            if (pagerinfo.CurrentPart < pagerinfo.PartsCount)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Next, isAjax, pageUrl);
            }

            //last
            if (pagerinfo.CurrentPart < pagerinfo.PartsCount)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Last, isAjax, pageUrl);
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        private static TagBuilder getPagerItem(SxPagerInfo pagerinfo, SxPagerItemType itemType, bool isAjax, Func<int, string> pageUrl = null, int? number = null)
        {
            int page = 1;
            string cssClass = null;
            switch (itemType)
            {
                case SxPagerItemType.First:
                    page = 1;
                    cssClass = "fa fa-angle-double-left";
                    break;
                case SxPagerItemType.Prev:
                    page = pagerinfo.Page - 1;
                    cssClass = "fa fa-caret-left";
                    break;
                case SxPagerItemType.Normal:
                    page = (int)number;
                    break;
                case SxPagerItemType.Next:
                    page = pagerinfo.Page + 1;
                    cssClass = "fa fa-caret-right";
                    break;
                case SxPagerItemType.Last:
                    page = pagerinfo.TotalPages;
                    cssClass = "fa fa-angle-double-right";
                    break;
            }

            var li = new TagBuilder("li");
            if (pagerinfo.Page == page && itemType == SxPagerItemType.Normal)
                li.AddCssClass("active");
            var a = new TagBuilder("a");

            if (isAjax)
            {
                a.MergeAttributes(new Dictionary<string, object>() {
                        { "href", "javascript:void(0)" },
                        { "data-page", page }
                    });
                if (pagerinfo.Page != page)
                    a.MergeAttribute("onclick", pagerinfo.FuncClick != null ? pagerinfo.FuncClick() : "clickPager(this)");
            }
            else
            {
                var href = pageUrl(page);
                a.MergeAttributes(new Dictionary<string, object>() {
                        { "href", pagerinfo.Page==page?"javascript:void(0)":href }
                    });
            }

            if (itemType != SxPagerItemType.Normal)
            {
                var span = new TagBuilder("span");
                span.AddCssClass(cssClass);
                a.InnerHtml += span;
            }
            else
            {
                a.InnerHtml += page;
            }
            li.InnerHtml += a;

            return li;
        }

        public class SxPagerInfo
        {
            public SxPagerInfo(int page, int pageSize)
            {
                Page = page;
                PageSize = pageSize;
                PagerSize = 10;
            }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int SkipCount
            {
                get
                {
                    var skip = ((Page - 1) * PageSize);
                    return skip < 0 ? 0 : skip;
                }
            }
            public int TotalItems { get; set; }
            public int TotalPages
            {
                get
                {
                    if (PageSize == 0) return 0;
                    var count = (int)Math.Ceiling((decimal)TotalItems / PageSize);
                    return count;
                }
            }

            public int PagerSize { get; set; }

            public int PartsCount
            {
                get
                {
                    return (int)Math.Ceiling((decimal)TotalPages / PagerSize);
                }
            }
            public int CurrentPart
            {
                get
                {
                    return (int)Math.Ceiling((decimal)Page / PagerSize);
                }
            }

            public Func<string> FuncClick { get; set; }
        }

        private enum SxPagerItemType : byte
        {
            Unknown = 0,
            First = 1,
            Prev = 2,
            Normal = 3,
            Next = 4,
            Last = 5
        }

        public sealed class SxPagedCollection<TModel>
        {
            public SxPagedCollection()
            {
                Collection = new TModel[0];
            }

            public TModel[] Collection { get; set; }
            public SxPagerInfo PagerInfo { get; set; }
            public int Length
            {
                get
                {
                    return this.Collection.Length;
                }
            }
        }
    }
}
