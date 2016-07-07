using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxLikeButtons(this HtmlHelper htmlHelper, SxLikeButton[] buttons)
        {
            var ul = new TagBuilder("ul");
            return MvcHtmlString.Create(ul.ToString());
        }
    }
}
