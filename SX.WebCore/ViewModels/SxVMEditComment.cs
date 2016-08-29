using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditComment
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        [Display(Name = "Представьтесь, пожалуйста*"), MaxLength(40), Required(ErrorMessage = "Введите имя, чтобы сотрудники знали, как к Вам обращаться")]
        public string UserName { get; set; }

        [DataType(DataType.MultilineText), Display(Name = "Текст комментария*"), Required(ErrorMessage = "Текст комментария не может быть пустым")]
        public string Html { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "Email*"), Required(ErrorMessage = "Введите адрес электронной почты"), MaxLength(50)]
        public string Email { get; set; }

        public string SecretCode { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
    }
}
