using SX.WebCore.ViewModels;
using System;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxThroughBanner(this HtmlHelper htmlHelper, SxVMBanner banner, Func<SxVMBanner, string> FuncBannerImgUrl, Func<SxVMBanner, string> FuncBannerUrl)
        {
            if (banner == null) return null;

            var div = new TagBuilder("div");
            div.AddCssClass("bnr");

            var figure = new TagBuilder("figure");
            figure.MergeAttribute("data-href", banner.Url);
            figure.MergeAttribute("data-id", banner.Id.ToString().ToLower());
            figure.AddCssClass(string.Concat("th-banner ", banner.Place.ToString().ToLower()));

            var a = new TagBuilder("a");
            a.MergeAttribute("href", FuncBannerUrl(banner));
            a.MergeAttribute("rel", "nofollow");
            a.MergeAttribute("target", "_blank");

            var img = new TagBuilder("img");
            img.MergeAttribute("alt", banner.Title);
            img.MergeAttribute("src", FuncBannerImgUrl(banner));
            a.InnerHtml += img.ToString(TagRenderMode.SelfClosing);

            figure.InnerHtml += a;

            div.InnerHtml += figure;

            if (!string.IsNullOrEmpty(banner.Description))
            {
                var desc = new TagBuilder("div");
                desc.AddCssClass("th-banner__desc");
                desc.InnerHtml += htmlHelper.Raw(banner.Description);
                div.InnerHtml += desc;
            }

            return MvcHtmlString.Create(div.ToString());
        }
    }
}
