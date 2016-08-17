using System.Web.Mvc;
using System.Web.Routing;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxRating(this HtmlHelper htmlHelper, int value, string funcAddRatingUrl, object htmlAttributes = null)
        {
            var div = new TagBuilder("div");

            if (htmlAttributes != null)
            {
                var attrs = new RouteValueDictionary(htmlAttributes);
                div.MergeAttributes(attrs);
            }
            div.AddCssClass("sx-rating");
            div.MergeAttribute("data-value", value.ToString());

            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-unstyled list-inline");

            TagBuilder li;
            TagBuilder a;
            for (int i = 1; i <= 10; i++)
            {
                li = new TagBuilder("li");

                a = new TagBuilder("a");
                a.MergeAttribute("data-url", funcAddRatingUrl + "?value=" + i.ToString());
                a.MergeAttribute("data-index", (i-1).ToString());
                a.MergeAttribute("href", "javascript:void(0)");

                if (value>=i)
                    a.InnerHtml += "<i class=\"fa fa-star\" aria-hidden=\"true\"></i>";
                else
                    a.InnerHtml += "<i class=\"fa fa-star-o\" aria-hidden=\"true\"></i>";
                li.InnerHtml += a;
                ul.InnerHtml += li;
            }
            div.InnerHtml += ul;


            return MvcHtmlString.Create(div.ToString());
        }
    }
}
