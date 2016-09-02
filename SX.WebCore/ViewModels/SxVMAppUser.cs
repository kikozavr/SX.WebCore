using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAppUser
    {
        public SxVMAppUser()
        {
            Roles = new SxVMAppRole[0];
        }

        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Никнейм")]
        public string NikName { get; set; }

        public string Email { get; set; }

        public SxVMAppRole[] Roles { get; set; }

        [Display(Name = "Аватар"), UIHint("PicturesLookupGrid")]
        public Guid? AvatarId { get; set; }
        public SxVMPicture Avatar { get; set; }

        public string RoleNames
        {
            get
            {
                if (!Roles.Any()) return null;

                var sb=new StringBuilder();
                for (int i = 0; i < Roles.Length; i++)
                {
                    sb.Append("; " + Roles[i].Name);
                }
                sb.Remove(0, 2);
                return sb.ToString();
            }
        }

        public bool IsOnline { get; set; }

        [Display(Name = "Сотрудник сайта")]
        public bool IsEmployee { get; set; }
    }
}