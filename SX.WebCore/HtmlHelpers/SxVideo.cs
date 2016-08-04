using SX.WebCore.Providers;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxVideo(this HtmlHelper htmlHelper, SxVideo video, SxVideoProvider.VideoQuality quality= SxVideoProvider.VideoQuality.Max, string url=null)
        {
            return MvcHtmlString.Create(GetVideoTemplate(video, quality, url));
        }

        public static string GetVideoTemplate(SxVideo video, SxVideoProvider.VideoQuality quality = SxVideoProvider.VideoQuality.Max, string url=null)
        {
            var id = video.Id.ToString().ToLower();
            var figure = new TagBuilder("figure");
            figure.AddCssClass("video");
            figure.MergeAttribute("style", string.Concat("background-image:url(", SxVideoProvider.GetVideoImageUrl(video.VideoId, quality), ");"));
            figure.MergeAttribute("id", id);

            var playWrapper = new TagBuilder("div");
            playWrapper.AddCssClass("v-p-wr");

            var a = new TagBuilder("a");
            if (url == null)
            {
                a.MergeAttribute("href", "javascript:void(0)");
                a.MergeAttribute("onclick", string.Concat("playVideoById('", id, "','", video.VideoId, "')"));
            }
            else
            {
                a.MergeAttribute("href", url);
            }
            a.InnerHtml += "<i class=\"fa fa-youtube-play\"></i>";
            playWrapper.InnerHtml += a;

            figure.InnerHtml += playWrapper;

            return figure.ToString();
        }
    }
}
