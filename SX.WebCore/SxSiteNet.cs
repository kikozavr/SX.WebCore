using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_NET")]
    public class SxSiteNet
    {
        [MaxLength(255)]
        public string Url { get; set; }

        public bool Show { get; set; }

        [Key]
        public int NetId { get; set; }
        public virtual SxNet Net { get; set; }
    }
}
