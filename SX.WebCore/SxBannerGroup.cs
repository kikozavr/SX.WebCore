using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_BANNER_GROUP")]
    public class SxBannerGroup : SxDbUpdatedModel<Guid>
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public virtual ICollection<SxBannerGroupBanner> BannerLinks { get; set; }
    }
}
