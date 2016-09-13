using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteError
    {
        public string RawUrl { get; set; }

        [AllowHtml]
        public string Html { get; set; }
    }
}
