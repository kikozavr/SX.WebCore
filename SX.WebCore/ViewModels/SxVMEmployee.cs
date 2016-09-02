using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEmployee
    {
        [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string Id { get; set; }

        public SxVMAppUser User { get; set; }

        public string Email
        {
            get
            {
                return User != null ? User.Email : null;
            }
            set { }
        }

        public string NikName
        {
            get
            {
                return User != null ? User.NikName : null;
            }
            set { }
        }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Описание"), AllowHtml, DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
