using System;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBanner
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public Guid PictureId { get; set; }

        public Guid? BannerGroupId { get; set; }

        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }

        public SxBanner.BannerPlace Place { get; set; }

        public int ClicksCount { get; set; }

        public int ShowsCount { get; set; }

        public decimal TargetCost { get; set; }

        public decimal CPM { get; set; }

        public decimal CTR
        {
            get
            {
                return Math.Round(ShowsCount == 0 ? 0 : (decimal)ClicksCount / ShowsCount * 100, 2);
            }
        }
    }
}
