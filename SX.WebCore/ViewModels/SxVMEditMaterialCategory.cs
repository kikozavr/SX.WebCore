using System;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public class SxVMEditMaterialCategory
    {
        [RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире"), Display(Name = "Идентификатор"), MaxLength(128)]
        public string Id { get; set; }

        public ModelCoreType ModelCoreType { get; set; }

        [Required, Display(Name = "Заголовок"), MaxLength(100)]
        public string Title { get; set; }

        public SxVMMaterialCategory ParentCategory { get; set; }
        public string ParentCategoryId { get; set; }

        [Display(Name = "Изображение"), UIHint("PicturesLookupGrid")]
        public Guid? FrontPictureId { get; set; }
        public SxVMPicture FrontPicture { get; set; }
    }
}
