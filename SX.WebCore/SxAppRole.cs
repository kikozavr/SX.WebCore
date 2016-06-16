using Microsoft.AspNet.Identity.EntityFramework;

namespace SX.WebCore
{
    public class SxAppRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
