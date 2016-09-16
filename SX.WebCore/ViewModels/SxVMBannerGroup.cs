using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBannerGroup
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(400, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Привязанные баннеры"), UIHint("_AddBanner")]
        public SxVMBanner[] Banners { get; set; }

        public SxVMBannerGroupBanner[] BannerLinks { get; set; }
    }
}
