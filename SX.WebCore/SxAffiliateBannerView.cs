using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_AFFILIATE_BANNER_VIEW")]
    public class SxAffiliateBannerView : SxDbModel<Guid>
    {
        public Guid BannerId { get; set; }
        public virtual SxBanner Banner { get; set; }

        public Guid AffiliatelinkId { get; set; }
        public virtual SxAffiliateLink AffiliateLink { get; set; }
    }
}
