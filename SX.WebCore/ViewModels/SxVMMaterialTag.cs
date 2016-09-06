using SX.WebCore.Attrubutes;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMMaterialTag
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Значение"), MaxWordsCount(4)]
        public string Id { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string Title { get; set; }

        public int Count { get; set; }

        public bool IsCurrent { get; set; }

        public int MaterialId { get; set; }

        public ModelCoreType ModelCoreType { get; set; }
    }
}
