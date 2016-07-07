using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_LIKE_BUTTON")]
    public class SxLikeButton : SxDbUpdatedModel<int>
    {
        public virtual SxNet Net { get; set; }
        public int NetId { get; set; }

        public bool Show { get; set; }

        public bool ShowCounter { get; set; }
    }
}
