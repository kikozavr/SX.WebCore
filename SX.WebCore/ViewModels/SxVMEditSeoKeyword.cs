using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditSeoKeyword
    {
        public int Id { get; set; }

        public SxVMSeoTags SeoTags { get; set; }

        [Required]
        public int SeoInfoId { get; set; }

        [MaxLength(50), Required, Display(Name = "Значение ключевого слова")]
        public string Value { get; set; }
    }
}
