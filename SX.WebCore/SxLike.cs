using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_LIKE")]
    public class SxLike : SxDbModel<Guid>
    {
        public LikeDirection Direction { get; set; }

        public virtual SxMaterial Material { get; set; }
        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        public enum LikeDirection : byte
        {
            Unknown = 0,

            /// <summary>
            /// Положительный лайк
            /// </summary>
            Up = 1,

            /// <summary>
            /// Отрицательный лайк
            /// </summary>
            Down = 2
        }
    }
}
