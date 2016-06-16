using System.Text;

namespace SX.WebCore.Providers
{
    public static class SxVideoProvider
    {
        public static string GetVideoImageUrl(string videoId, VideoQuality quality = VideoQuality.Max)
        {
            var sb = new StringBuilder();
            sb.Append("http://img.youtube.com/vi/");
            sb.Append(videoId);

            switch(quality)
            {
                case VideoQuality.Max:
                    sb.Append("/maxresdefault.jpg");
                    break;
                case VideoQuality.Standart:
                    sb.Append("/sddefault.jpg");
                    break;
                case VideoQuality.Medium:
                    sb.Append("/mqdefault.jpg");
                    break;
                case VideoQuality.High:
                    sb.Append("/hqdefault.jpg");
                    break;
                case VideoQuality.Default:
                    sb.Append("/default.jpg");
                    break;
            }

            return sb.ToString();
        }

        public enum VideoQuality : byte
        {
            Unknown=0,
            Max=1,
            Standart=2,
            Medium=3,
            High=4,
            Default=5
        }
    }
}
