using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSeoKeyword
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Значение ключевого слова")]
        public string Value { get; set; }

        [Required]
        public int SeoTagsId { get; set; }
        public SxVMSeoTags SeoTags { get; set; }
    }
}
