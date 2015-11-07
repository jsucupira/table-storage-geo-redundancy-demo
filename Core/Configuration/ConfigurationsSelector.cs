using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public static class ConfigurationsSelector
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;

        public static ConfigurationItems GetAll()
        {
            return ConfigurationContextFactory.Create().GetAll();
        }

        public static bool GetBooleanSetting(string settingKey)
        {
            string rawValue = GetSetting(settingKey);
            bool value;
            if (bool.TryParse(rawValue, out value))
                return value;
            if (rawValue == "0")
                return false;
            if (rawValue == "1")
                return true;
            return false;
        }

        public static double GetDoubleSetting(string settingKey)
        {
            string rawValue = GetSetting(settingKey);
            double value;
            if (double.TryParse(rawValue, out value))
                return value;
            throw new Exception(string.Format("Could not convert Configuration item '{0}' ({1}) to Double.", settingKey, rawValue));
        }

        public static int GetIntegerSetting(string settingKey)
        {
            string rawValue = GetSetting(settingKey);
            int value;
            if (int.TryParse(rawValue, out value))
                return value;
            throw new Exception(string.Format("Could not convert Configuration item '{0}' ({1}) to Integer.", settingKey, rawValue));
        }

        public static string GetSetting(string settingKey)
        {
            string cacheKey = settingKey;
            string value = _cache.Get(cacheKey) as string;
            if (value != null)
                return value;

            IConfigurationContext context = ConfigurationContextFactory.Create();
            ConfigurationItem item = context.GetItem(settingKey);
            if (item == null)
                throw new Exception(string.Format("Configuration Setting '{0}' not found.", settingKey));

            SetCache(cacheKey, item.Value);

            return item.Value;
        }

        public static TimeSpan GetTimeSpanSetting(string settingKey)
        {
            string setting = GetSetting(settingKey);
            return TimeSpan.Parse(setting);
        }

        #region Caching

        public static void RefreshCache()
        {
            Task.Factory.StartNew(() =>
            {
                foreach (KeyValuePair<string, object> item in _cache)
                    _cache.Remove(item.Key);
            });
        }

        #endregion

        public static void SaveSetting(string key, string value)
        {
            IConfigurationContext context = ConfigurationContextFactory.Create();
            context.SaveItem(key, value);
            SetCache(key, value);
        }

        private static void SetCache(string key, object value)
        {
            DateTimeOffset dateTime = DateTimeOffset.Now.AddMinutes(30);
            _cache.Set(key, value, new CacheItemPolicy {AbsoluteExpiration = dateTime});
        }
    }
}