using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxShareButtons(this HtmlHelper htmlHelper, SxShareButton[] buttons)
        {
            if (buttons == null || !buttons.Any()) return null;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("share-buttons");

            SxShareButton button;
            TagBuilder li;
            TagBuilder btn;
            for (int i = 0; i < buttons.Length; i++)
            {
                button = buttons[i];

                li = new TagBuilder("li");
                li.MergeAttribute("data-type", button.Net.Code);
                li.MergeAttribute("tabindex", i.ToString());
                li.AddCssClass("goodshare");

                btn = new TagBuilder("button");
                btn.MergeAttribute("style", "background-color:"+button.Net.Color);
                btn.AddCssClass("btn btn-sm btn-default");
                btn.InnerHtml += "<span class=\"share-buttons__icon\"><i class=\"" + button.Net.LogoCssClass + "\"></i></span>";
                btn.InnerHtml += "<span class=\"share-buttons__title\">" + button.Net.Name+"</span>";
                if(button.Net.HasCounter && button.ShowCounter)
                    btn.InnerHtml += "<span class=\"badge share-buttons__counter\" data-counter=\"" + button.Net.Code+"\"></span>";
                li.InnerHtml += btn;

                ul.InnerHtml += li;
            }

            return MvcHtmlString.Create(ul.ToString());
        }
    }
}
