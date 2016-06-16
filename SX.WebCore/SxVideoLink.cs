using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_VIDEO_LINK")]
    public class SxVideoLink
    {
        public virtual SxMaterial Material { get; set; }

        [Key, Column(Order = 1)]
        public int MaterialId { get; set; }

        [Key, Column(Order = 2)]
        public ModelCoreType ModelCoreType { get; set; }

        public SxVideo Video { get; set; }

        [Key, Column(Order = 3)]
        public Guid VideoId { get; set; }
    }
}
