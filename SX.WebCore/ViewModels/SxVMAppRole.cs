using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAppRole
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "IdentityStringField")]
        [Display(Name= "Наименование")]
        public string Name { get; set; }

        [MaxLength(256, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}