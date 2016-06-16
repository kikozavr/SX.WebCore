using SX.WebCore.Providers;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxVideo(this HtmlHelper htmlHelper, SxVideo video, SxVideoProvider.VideoQuality quality= SxVideoProvider.VideoQuality.Max)
        {
            return MvcHtmlString.Create(GetVideoTemplate(video, quality));
        }

        public static string GetVideoTemplate(SxVideo video, SxVideoProvider.VideoQuality quality = SxVideoProvider.VideoQuality.Max)
        {
            var id = video.Id.ToString().ToLower();
            var figure = new TagBuilder("figure");
            figure.AddCssClass("video");
            figure.MergeAttribute("style", string.Concat("background-image:url(", SxVideoProvider.GetVideoImageUrl(video.VideoId, quality), ");"));
            figure.MergeAttribute("id", id);

            var playWrapper = new TagBuilder("div");
            playWrapper.AddCssClass("v-p-wr");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", "javascript:void(0)");
            a.MergeAttribute("onclick", string.Concat("playVideoById('", id, "','", video.VideoId,"')"));
            a.InnerHtml += "<i class=\"fa fa-youtube-play\"></i>";
            playWrapper.InnerHtml += a;

            figure.InnerHtml += playWrapper;

            return figure.ToString();
        }
    }
}
