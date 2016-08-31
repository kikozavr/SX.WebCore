using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBannerGroup
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public SxVMBannerGroupBanner[] BannerLinks { get; set; }
    }
}
