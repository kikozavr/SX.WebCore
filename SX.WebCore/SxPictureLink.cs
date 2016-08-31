using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_PICTURE_LINK")]
    public class SxPictureLink
    {
        [Key, Column(Order = 1)]
        public int MaterialId { get; set; }

        [Key, Column(Order = 2)]
        public ModelCoreType ModelCoreType { get; set; }

        public virtual SxMaterial Material { get; set; }

        [Key, Column(Order = 3)]
        public Guid PictureId { get; set; }

        public virtual SxPicture Picture { get; set; }
    }
}
