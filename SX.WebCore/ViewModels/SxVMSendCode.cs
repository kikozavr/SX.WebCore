using System.Collections.Generic;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSendCode
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
