using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMExternalLoginConfirmation
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
