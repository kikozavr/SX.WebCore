using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestSubject
    {
        public int Id { get; set; }

        [MaxLength(400, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Заголовок"), DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Display(Name = "Описание"), AllowHtml, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public SxVMSiteTest Test { get; set; }
        public int TestId { get; set; }

        public SxVMPicture Picture { get; set; }
        [Display(Name = "Картинка"), UIHint("PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
    }
}
