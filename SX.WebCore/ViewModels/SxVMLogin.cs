using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public class SxVMLogin
    {
        [Required(ErrorMessage ="Требуется Email")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
