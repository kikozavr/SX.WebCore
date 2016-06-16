using System.Text;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using SX.WebCore.ViewModels;
using System.Web;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxTagsCloud(this HtmlHelper htmlHelper, SxVMMaterialTag[] tags, string url, int maxFontSize=30)
        {
            var sb = new StringBuilder();
            var list = new Dictionary<string, int>();
            var otherTags = tags.Where(x => !x.IsCurrent).OrderByDescending(x => x.Count).Select(x=>x.Title).ToArray();

            for (int i = 0; i < otherTags.Length; i++)
            {
                list.Add(otherTags[i], maxFontSize-(i+ 7));
            }

            sb.Append("<ul class=\"tags-cloud\">");
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                var fs = tag.IsCurrent ? maxFontSize : (list[tag.Title]>10? list[tag.Title]:10);
                writeItem(sb, tag, url, maxFontSize, fs);
            }
            sb.Append("</ul>");

            return MvcHtmlString.Create(sb.ToString());
        }

        private static void writeItem(StringBuilder sb, SxVMMaterialTag tag, string url, int maxFontSize, int fs)
        {
            sb.Append("<li ");
            if(tag.IsCurrent)
                sb.Append("class=\"current\" ");
            sb.Append("style=\"font-size:" + fs + "px\">");
            sb.Append("<a style=\"line-height:"+ maxFontSize + "px;\" href=\"");
            sb.Append(url+"?tag="+ HttpUtility.UrlEncode(tag.Title.ToLowerInvariant()));
            sb.Append("\"");
            sb.Append(">");
            sb.Append(tag.Title);
            sb.Append("</a></li>");
        }
    }
}
