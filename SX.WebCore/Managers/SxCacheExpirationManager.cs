using System;
using System.Runtime.Caching;

namespace SX.WebCore.Managers
{
    public static class SxCacheExpirationManager
    {
        public static CacheItemPolicy GetExpiration(int minutes)
        {
            return new CacheItemPolicy { AbsoluteExpiration= DateTimeOffset.Now.AddMinutes(minutes) };
        }
    }
}
