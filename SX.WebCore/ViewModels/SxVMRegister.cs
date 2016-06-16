using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMRegister
    {
        [Required(ErrorMessage = "Необходимо ввести адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Необходимо ввести Ваш никнейм"), MaxLength(100)]
        [Display(Name = "Ник-нейм*")]
        public string NikName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно быть больше {2} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль*")]
        [Compare("Password", ErrorMessage = "Введенные пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
