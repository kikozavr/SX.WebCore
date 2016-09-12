using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    [MetadataType(typeof(SxVMMaterialMetadata))]
    public class SxVMMaterial
    {
        public SxVMMaterial()
        {
            Videos = new SxVideo[0];
            MaterialTags = new SxVMMaterialTag[0];
        }

        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        public DateTime DateOfPublication { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        public string Title { get; set; }

        public string TitleUrl { get; set; }
        
        public string Html { get; set; }
        
        public string Foreword { get; set; }

        public bool Show { get; set; }
        
        public Guid? FrontPictureId { get; set; }
        public virtual SxVMPicture FrontPicture { get; set; }

        public bool ShowFrontPictureOnDetailPage { get; set; }

        public int ViewsCount { get; set; }

        public virtual SxVMAppUser User { get; set; }
        public string UserId { get; set; }

        public virtual SxVMSeoTags SeoTags { get; set; }
        public int? SeoTagsId { get; set; }

        public virtual SxVMMaterialCategory Category { get; set; }
        public string CategoryId { get; set; }

        public SxVideo[] Videos { get; set; }

        public int LikeUpCount { get; set; }

        public int LikeDownCount { get; set; }

        public virtual string GetUrl(UrlHelper urlHelper)
        {
            return "#";
        }

        public string GetForewordFromHtml(int maxLettersCount)
        {
            return Regex.Replace(Html.Length <= maxLettersCount ? Html : Html.Substring(0, maxLettersCount) + "...", "<.*?>", string.Empty);
        }

        public int CommentsCount { get; set; }

        public int Rating { get; set; }

        public string SourceUrl { get; set; }

        public bool IsTop { get; set; }

        public SxVMMaterialTag[] MaterialTags { get; set; }
    }

    public class SxVMMaterialMetadata
    {
        [UIHint("EditDate"), Display(Name ="Дата публикации")]
        public DateTime DateOfPublication { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name ="Заголовок материала")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Строковый ключ")]
        public string TitleUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [DataType(DataType.MultilineText), AllowHtml, Display(Name ="Содержание материала")]
        public string Html { get; set; }

        [DataType(DataType.MultilineText), Display(Name ="Краткое описание материала")]
        public string Foreword { get; set; }

        [Display(Name ="Возможность просмотра")]
        public bool Show { get; set; }

        [UIHint("PicturesLookupGrid"), Display(Name ="Картинка")]
        public Guid? FrontPictureId { get; set; }

        [Display(Name = "Показывать на странице материала")]
        public bool ShowFrontPictureOnDetailPage { get; set; }

        [Display(Name = "Категория материала"), UIHint("MaterialCategoryLookupGrid")]
        public string CategoryId { get; set; }
    }
}
