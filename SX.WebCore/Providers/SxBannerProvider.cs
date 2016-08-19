using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.WebCore.Providers
{
    public class SxBannerProvider
    {
        private static Func<SxBannerCollection> _collection;
        public SxBannerProvider(Func<SxBannerCollection> collection)
        {
            _collection = collection;
        }

        public SxBannerCollection BannerCollection
        {
            get
            {
                return _collection();
            }
        }

        public SxBanner[] GetPageBanners(string rawUrl)
        {
            var list = new List<SxBanner>();

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

        private static SxBanner getBanner(SxBanner.BannerPlace place, List<SxBanner> existBanners, string rawUrl = null)
        {
            var banner = getPlaceBanner(place, existBanners, rawUrl);
            return banner;
        }

        private static SxBanner getPlaceBanner(SxBanner.BannerPlace place, List<SxBanner> existBanners, string rawUrl = null)
        {
            SxBanner banner = null;
            var data = _collection().Banners.Where(x => (x.Place == place || (x.Place == place && x.RawUrl == rawUrl)) && existBanners.SingleOrDefault(b=>b.PictureId==x.PictureId)==null).ToArray();
            banner = getRandomBanner(data, rawUrl);
            return banner;
        }

        private static SxBanner getRandomBanner(SxBanner[] data, string rawUrl = null)
        {
            SxBanner banner = null;
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
