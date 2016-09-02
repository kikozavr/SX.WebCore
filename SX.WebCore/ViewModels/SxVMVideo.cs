using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMVideo
    {
        public Guid Id { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Название видео")]
        public string Title { get; set; }

        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Идентификатор видео")]
        public string VideoId { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ValidUrlField")]
        [Display(Name = "Источник видео")]
        public string SourceUrl { get; set; }
    }
}
