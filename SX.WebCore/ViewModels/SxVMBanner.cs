using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBanner
    {
        public Guid Id { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ValidUrlField")]
        [Display(Name = "Ссылка")]
        public string Url { get; set; }

        [MaxLength(266, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Внутренний адрес страницы")]
        public string RawUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Картинка"), UIHint("_PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
        public SxVMPicture Picture { get; set; }

        [Display(Name = "Описание"), DataType(DataType.MultilineText), AllowHtml]
        public string Description { get; set; }

        public Guid? BannerGroupId { get; set; }

        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }

        [Display(Name = "Место на странице")]
        public SxBanner.BannerPlace? Place { get; set; }

        [Display(Name = "Тип баннера")]
        public SxBanner.BannerType? Type { get; set; }

        public int ClicksCount { get; set; }

        public int ShowsCount { get; set; }

        public decimal TargetCost { get; set; }

        public decimal CPM { get; set; }

        public decimal CTR { get; set; }
    }
}
