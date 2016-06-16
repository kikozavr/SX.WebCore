using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_COMMENT")]
    public class SxComment : SxDbUpdatedModel<int>, ISxHasHtml
    {
        public virtual SxMaterial Material { get; set; }
        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        public virtual SxAppUser User { get; set; }
        [MaxLength(128), Index]
        public string UserId { get; set; }

        [MaxLength(50), Index]
        public string UserName { get; set; }

        public string Html { get; set; }

        [MaxLength(50), Index]
        public string Email { get; set; }
    }
}
