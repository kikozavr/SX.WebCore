using SX.WebCore.ViewModels;
using System;
using System.Linq;
using SX.WebCore.Extantions;

namespace SX.WebCore.Providers
{
    public class SxSiteNetProvider
    {
        private static object _locker = new object();

        public SxSiteNetProvider(Func<SxVMSiteNet[]> siteNetsFunc)
        {
            SiteNets = new SxVMSiteNet[0];
            if (siteNetsFunc != null)
                SiteNets = siteNetsFunc();
        }

        private SxVMSiteNet[] _siteNets;
        public SxVMSiteNet[] SiteNets
        {
            get { return _siteNets; }
            set { _siteNets = value; }
        }

        public void UpdateInCache(SxVMSiteNet model)
        {
            if (!_siteNets.Any()) return;

            lock (_locker)
            {
                var index = Array.FindIndex(_siteNets, x => { return x.NetId == model.NetId; });
                if(index==-1 && model.Show)
                {
                    Array.Resize(ref _siteNets, _siteNets.Length + 1);
                    _siteNets[_siteNets.Length - 1] = model;
                    return;
                }

                if(!model.Show)
                {
                    ArrayExtantions.RemoveAt(ref _siteNets, index);
                }
                else
                {
                    _siteNets[index] = model;
                }
            }
        }
    }
}
