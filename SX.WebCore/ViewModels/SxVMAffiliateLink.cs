using System;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAffiliateLink
    {
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        public string RawUrl { get; set; }

        public string Description { get; set; }

        public int ViewsCount { get; set; }

        public decimal ClickCost { get; set; }
    }
}
