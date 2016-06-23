using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditBannerGroup
    {
        public SxVMEditBannerGroup()
        {
            Banners = new SxVMBanner[0];
        }

        public Guid Id { get; set; }

        [Required, MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(400), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Привязанные баннеры"), UIHint("AddBanner")]
        public SxVMBanner[] Banners { get; set; }
    }
}
