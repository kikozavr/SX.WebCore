using System;
using System.Collections.Generic;

namespace SX.WebCore.Providers
{
    public sealed class SxSiteSettingsProvider
    {
        private Func<Dictionary<string, SxSiteSetting>> _settings;
        public SxSiteSettingsProvider(Func<Dictionary<string, SxSiteSetting>> settings)
        {
            _settings = settings;
        }

        public SxSiteSetting Get(string key)
        {
            return _settings == null || !_settings().ContainsKey(key) ? null : _settings()[key];
        }

        public void Set(string key, string value)
        {
            if (_settings == null || !_settings().ContainsKey(key)) return;
            _settings()[key].Value = value;
        }
    }
}
