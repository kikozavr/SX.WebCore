using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_PLACE")]
    public class SxPlace : SxDbUpdatedModel<int>
    {
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string TitleUrl { get; set; }

        public int? ParentPlaceId { get; set; }
        public virtual SxPlace ParentPlace { get; set; }
    }
}
