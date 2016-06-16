using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_BANNED_URL")]
    public sealed class SxBannedUrl : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(255)]
        public string Url { get; set; }

        [Required, MaxLength(255)]
        public string Couse { get; set; }
    }
}
