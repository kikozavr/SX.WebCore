using SX.WebCore.ViewModels;
using System;

namespace SX.WebCore.Providers
{
    public class SxSiteNetProvider
    {
        public SxSiteNetProvider(Func<SxVMSiteNet[]> siteNetsFunc)
        {
            SiteNets = new SxVMSiteNet[0];
            if (siteNetsFunc != null)
                SiteNets = siteNetsFunc();
        }

        public SxVMSiteNet[] SiteNets { get; set; }
    }
}
