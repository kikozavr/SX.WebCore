using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMBannedUrl
    {
        public DateTime DateCreate { get; set; }

        [Display(Name = "Адрес")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Url(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ValidUrlField")]
        [Display(Name = "Адрес")]
        public string Url { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Причина бана"), DataType(DataType.MultilineText)]
        public string Couse { get; set; }
    }
}
