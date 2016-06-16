namespace SX.WebCore
{
    public sealed class SxBannerCollection
    {
        public SxBannerCollection()
        {
            Banners = new SxBanner[0];
            BannerGroups = new SxBannerGroup[0];
        }

        public SxBanner[] Banners { get; set; }
        public SxBannerGroup[] BannerGroups { get; set; }
    }
}
