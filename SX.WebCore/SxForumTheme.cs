using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_FORUM_THEME")]
    public class SxForumTheme : SxMaterial
    {
        public virtual SxForumPart Part { get; set; }
        public int PartId { get; set; }
    }
}
