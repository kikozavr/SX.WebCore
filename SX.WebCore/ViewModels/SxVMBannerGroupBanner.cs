using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBannerGroupBanner
    {
        public SxVMBanner Banner { get; set; }
        public Guid BannerId { get; set; }

        public SxVMBannerGroup BannerGroup { get; set; }
        public Guid BannerGroupId { get; set; }
    }
}
