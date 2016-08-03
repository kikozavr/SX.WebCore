using System;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxThroughBanner(this HtmlHelper htmlHelper, SxBanner banner, Func<SxBanner, string> FuncBannerImgUrl)
        {
            if (banner == null) return null;

            var div = new TagBuilder("div");

            var figure = new TagBuilder("figure");
            figure.MergeAttribute("data-href", banner.Url);
            figure.MergeAttribute("data-id", banner.Id.ToString().ToLower());
            figure.AddCssClass(string.Concat("th-banner ", banner.Place.ToString().ToLower()));

            var a = new TagBuilder("a");
            a.MergeAttribute("href", "javascript:void(0)");
            a.MergeAttribute("rel", "nofollow");

            var img = new TagBuilder("img");
            img.MergeAttribute("alt", banner.Title);
            img.MergeAttribute("src", FuncBannerImgUrl(banner));
            a.InnerHtml += img.ToString(TagRenderMode.SelfClosing);

            figure.InnerHtml += a;

            div.InnerHtml += figure;

            if (!string.IsNullOrEmpty(banner.Description))
            {
                var p = new TagBuilder("p");
                p.AddCssClass("th-banner__desc");
                p.InnerHtml += banner.Description;
                div.InnerHtml += p;
            }

            return MvcHtmlString.Create(div.ToString());
        }
    }
}
