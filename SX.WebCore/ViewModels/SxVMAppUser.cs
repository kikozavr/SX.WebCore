using System;
using System.Linq;
using System.Text;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAppUser
    {
        public string Id { get; set; }
        public string NikName { get; set; }
        public string Email { get; set; }
        public SxVMAppRole[] Roles { get; set; }
        public Guid? AvatarId { get; set; }
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
    }
}