using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditAppUser
    {
        public SxVMEditAppUser()
        {
            Roles = new SxVMAppRole[0];
        }

        public string Id { get; set; }

        public SxPicture Avatar { get; set; }
        [Display(Name = "Аватар"), UIHint("EditImage")]
        public Guid? AvatarId { get; set; }

        public string Email { get; set; }

        [Display(Name = "Никнейм"), MaxLength(50)]
        public string NikName { get; set; }

        public SxVMAppRole[] Roles { get; set; }
        public bool IsOnline { get; set; }

        [Display(Name = "Сотрудник сайта")]
        public bool IsEmployee { get; set; }
    }
}
