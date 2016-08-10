using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_AFFILIATE_LINK")]
    public class SxAffiliateLink : SxDbUpdatedModel<Guid>, IHasViewsCount
    {
        public string Description { get; set; }

        public int ViewsCount { get; set; }

        public decimal ClickCost { get; set; }
    }
}
