using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [NotMapped]
    public sealed class SxBannersStatistic
    {
        public int ClicksCount { get; set; }
        public int ShowsCount { get; set; }
        public string Url { get; set; }
    }
}
