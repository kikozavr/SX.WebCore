using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SX.WebCore.Providers
{
    public class SxBannerProvider
    {
        private static Func<SxBanner[]> _banners;
        public SxBannerProvider(Func<SxBanner[]> banners)
        {
            _banners = banners;
        }

        private static SxBanner getPlaceBanner(SxBanner.BannerPlace place)
        {
            SxBanner banner = null;
            var data = _banners().Where(x => x.Place == place && x.ControllerName == null && x.ActionName == null).ToArray();
            banner = getRandomBanner(data);
            return banner;
        }

        private static SxBanner getControllerBanner(SxBanner.BannerPlace place, string controllerName)
        {
            SxBanner banner = null;
            var data = _banners().Where(x => x.Place == place && x.ControllerName == controllerName && x.ActionName == null).ToArray();
            banner = getRandomBanner(data);
            return banner ?? getPlaceBanner(place);
        }

        private static SxBanner getActionBanner(SxBanner.BannerPlace place, string controllerName, string actionName)
        {
            SxBanner banner = null;
            var data = _banners().Where(x => x.Place == place && Equals(x.ControllerName, controllerName) && Equals(x.ActionName, actionName)).ToArray();
            banner = getRandomBanner(data);
            return banner ?? getControllerBanner(place, controllerName);
        }

        private static SxBanner getRandomBanner(SxBanner[] data)
        {
            SxBanner banner = null;
            if (data.Any())
            {
                var random = new Random();
                var randomIndex = random.Next(data.Length);
                banner = data[randomIndex];
            }

            return banner;
        }

        private static SxBanner getBanner(SxBanner.BannerPlace place, string controllerName = null, string actionName = null)
        {
            if (place == SxBanner.BannerPlace.Unknown) return null;

            SxBanner banner = null;
            if (controllerName == null && actionName == null)
                banner = getPlaceBanner(place);
            else if (controllerName != null && actionName == null)
                banner = getControllerBanner(place, controllerName);
            else if (controllerName != null && actionName != null)
                banner = getActionBanner(place, controllerName, actionName);

            return banner;
        }

        public SxBanner[] GetPageBanners<TDbContext>(string controllerName, string actionName) where TDbContext : SxDbContext
        {
            var list = new List<SxBanner>();
            foreach (var p in Enum.GetValues(typeof(SxBanner.BannerPlace)))
            {
                var place = (SxBanner.BannerPlace)p;
                var banner = getBanner(place, controllerName, actionName);
                if (banner != null)
                    list.Add(banner);
            }

            Task.Run(() =>
            {
                var keys = list.Select(x => x.Id).ToArray();
                new RepoBanner<TDbContext>().AddShows(keys);
            });

            return list.ToArray();
        }
    }
}
