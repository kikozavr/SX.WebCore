using System;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMComment
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        [Required(ErrorMessage = "Текст комментария не может быть пустым")]
        [DataType(DataType.MultilineText), Display(Name = "Текст комментария*")]
        public string Html { get; set; }

        [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string UserId { get; set; }
        public SxVMAppUser User { get; set; }

        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Required(ErrorMessage = "Введите имя, чтобы сотрудники знали, как к Вам обращаться")]
        [Display(Name = "Представьтесь, пожалуйста*")]
        public string UserName { get; set; }

        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ValidEmailField")]
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        public string SecretCode { get; set; }
    }
}
