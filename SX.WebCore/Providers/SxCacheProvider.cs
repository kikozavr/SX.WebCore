using SX.WebCore.Managers;
using System.Runtime.Caching;

namespace SX.WebCore.Providers
{
    public class SxCacheProvider
    {
        private MemoryCache _cache;
        public SxCacheProvider()
        {
            if(_cache==null)
                _cache = new MemoryCache("APPLICATION_CACHE");
        }

        public TModel Get<TModel>(string key)
        {
            return (TModel)_cache.Get(key);
        }

        public TModel Set<TModel>(string key, TModel model, int cacheExpiration)
        {
            _cache.Add(key, model, SxCacheExpirationManager.GetExpiration(minutes: cacheExpiration));
            return model;
        }
    }
}
