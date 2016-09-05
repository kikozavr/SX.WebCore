using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_NET")]
    public class SxNet : SxDbModel<int>
    {
        [MaxLength(2), Required, Index]
        public string Code { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(50), Required]
        public string Url { get; set; }

        [MaxLength(50), Required]
        public string LogoCssClass { get; set; }

        [MaxLength(20), Required]
        public string Color { get; set; }

        public bool HasCounter { get; set; }

        [MaxLength(255)]
        public string SiteUrl { get; set; }

        public virtual SxSiteNet SiteNet { get; set; }
    }
}
