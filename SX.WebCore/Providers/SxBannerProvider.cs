using SX.WebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.WebCore.Providers
{
    public class SxBannerProvider
    {
        private static Func<SxVMBannerCollection> _collection;
        public SxBannerProvider(Func<SxVMBannerCollection> collection)
        {
            _collection = collection;
        }

        public SxVMBannerCollection BannerCollection
        {
            get
            {
                return _collection();
            }
        }

        public SxVMBanner[] GetPageBanners(string rawUrl)
        {
            var list = new List<SxVMBanner>();

            foreach (var p in Enum.GetValues(typeof(SxBanner.BannerPlace)))
            {
                var place = (SxBanner.BannerPlace)p;
                if (place == SxBanner.BannerPlace.Unknown) continue;

                var banner = getBanner(place, list, rawUrl);
                if (banner != null)
                    list.Add(banner);
            }

            return list.ToArray();
        }

        private static SxVMBanner getBanner(SxBanner.BannerPlace place, List<SxVMBanner> existBanners, string rawUrl = null)
        {
            var banner = getPlaceBanner(place, existBanners, rawUrl);
            return banner;
        }

        private static SxVMBanner getPlaceBanner(SxBanner.BannerPlace place, List<SxVMBanner> existBanners, string rawUrl = null)
        {
            SxVMBanner banner = null;
            var data = _collection().Banners.Where(x => (x.Place == place || (x.Place == place && x.RawUrl == rawUrl)) && existBanners.SingleOrDefault(b=>b.PictureId==x.PictureId)==null).ToArray();
            banner = getRandomBanner(data, rawUrl);
            return banner;
        }

        private static SxVMBanner getRandomBanner(SxVMBanner[] data, string rawUrl = null)
        {
            SxVMBanner banner = null;
            if (data.Any())
            {
                var dataForRawUrl = data.Where(x => x.RawUrl == rawUrl).ToArray();
                if (dataForRawUrl.Any())
                {
                    var randomForRawUrl = new Random();
                    var randomIndexForRawUrl = randomForRawUrl.Next(dataForRawUrl.Length);
                    banner = dataForRawUrl[randomIndexForRawUrl];
                }
                else
                {
                    var random = new Random();
                    var randomIndex = random.Next(data.Length);
                    banner = data[randomIndex];
                }
            }

            return banner;
        }
    }
}
