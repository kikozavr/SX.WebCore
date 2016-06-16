using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_BANNER_GROUP_LINK")]
    public class SxBannerGroupBanner
    {
        public virtual SxBanner Banner { get; set; }
        public Guid BannerId { get; set; }

        public virtual SxBannerGroup BannerGroup { get; set; }
        public Guid BannerGroupId { get; set; }
    }
}
