using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public class SxAppUser : IdentityUser
    {
        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        [MaxLength(100)]
        public string NikName { get; set; }

        public virtual SxPicture Avatar { get; set; }
        public Guid? AvatarId { get; set; }

        public SxAppUser() { }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SxAppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
