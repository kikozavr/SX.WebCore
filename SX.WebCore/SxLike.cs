using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_LIKE")]
    public class SxLike : SxDbModel<Guid>
    {
        public Enums.LikeDirection Direction { get; set; }

        public virtual SxUserClick UserClick { get; set; }
        public Guid UserClickId { get; set; }
    }
}
