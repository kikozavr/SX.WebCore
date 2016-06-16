using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_USER_CLICK")]
    public class SxUserClick : SxDbModel<Guid>
    {
        public Enums.UserClickType ClickType { get; set; }

        [MaxLength(128), Required, Index]
        public string SessionId { get; set; }

        public virtual SxAppUser User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public virtual SxMaterial Material { get; set; }
        public int MaterialId { get; set; }
        public Enums.ModelCoreType? ModelCoreType { get; set; }
    }
}
