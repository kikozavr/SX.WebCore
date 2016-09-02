using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public class SxVMMaterialCategory : IHierarchy<SxVMMaterialCategory>
    {
        public SxVMMaterialCategory()
        {
            ChildCategories = new SxVMMaterialCategory[0];
        }

        [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "IdentityStringField")]
        [Display(Name = "Идентификатор")]
        public string Id { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required, Display(Name = "Заголовок")]
        public string Title { get; set; }

        public ModelCoreType ModelCoreType { get; set; }

        public virtual SxVMMaterialCategory ParentCategory { get; set; }
        public string ParentCategoryId { get; set; }

        public virtual SxVMPicture FrontPicture { get; set; }
        [Display(Name = "Изображение"), UIHint("PicturesLookupGrid")]
        public Guid? FrontPictureId { get; set; }

        public int Level { get; set; }

        public SxVMMaterialCategory[] ChildCategories { get; set; }
    }
}
