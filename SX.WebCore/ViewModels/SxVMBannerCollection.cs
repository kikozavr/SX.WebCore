namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBannerCollection
    {
        public SxVMBannerCollection()
        {
            Banners = new SxVMBanner[0];
            BannerGroups = new SxVMBannerGroup[0];
        }

        public SxVMBanner[] Banners { get; set; }
        public SxVMBannerGroup[] BannerGroups { get; set; }
    }
}
