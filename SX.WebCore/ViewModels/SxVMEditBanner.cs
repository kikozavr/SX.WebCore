using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditBanner
    {
        public Guid Id { get; set; }

        [MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        public SxVMPicture Picture { get; set; }
        [Required, Display(Name = "Картинка"), UIHint("PicturesLookupGrid")]
        public Guid? PictureId { get; set; }

        [Required, MaxLength(255), Display(Name = "Ссылка"), DataType(DataType.Url)]
        public string Url { get; set; }

        [Display(Name = "Место на странице")]
        public SxBanner.BannerPlace? Place { get; set; }

        [MaxLength(266), Display(Name = "Внутренний адрес страницы")]
        public string RawUrl { get; set; }

        [Display(Name ="Описание"), DataType(DataType.MultilineText), AllowHtml]
        public string Description { get; set; }
    }
}
