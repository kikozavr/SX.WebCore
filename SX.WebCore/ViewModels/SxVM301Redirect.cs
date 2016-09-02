using SX.WebCore.Attrubutes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVM301Redirect
    {
        public Guid? Id { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType =typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Старый адрес")]
        public string OldUrl { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Новый адрес")]
        [NotEqual("OldUrl", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "NotEqualField")]
        public string NewUrl { get; set; }
    }
}
