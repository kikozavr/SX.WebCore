using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_MATERIAL_CATEGORY")]
    public class SxMaterialCategory : SxDbModel<string>
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        public ModelCoreType ModelCoreType { get; set; }

        public virtual SxMaterialCategory ParentCategory { get; set; }
        public string ParentCategoryId { get; set; }

        public virtual SxPicture FrontPicture { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}
