using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_RATING")]
    public class SxRating : SxDbModel<int>
    {
        [MaxLength(128)]
        public string UserId { get; set; }
        public virtual SxAppUser User { get; set; }

        public int Value { get; set; }

        [MaxLength(128)]
        public string SessionId { get; set; }

        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public virtual SxMaterial Material { get; set; }
    }
}
