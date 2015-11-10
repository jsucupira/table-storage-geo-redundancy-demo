using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.Azure;

namespace Core.Configuration
{
    public static class ConfigurationsSelector
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;

        public static List<ConfigurationItem> GetAll()
        {
            List<ConfigurationItem> items;
            if (_cache.Any())
            {
                items = _cache.Select(t => new ConfigurationItem
                {
                    Key = t.Key,
                    Value = t.Value.ToString()
                }).ToList();
                return items;
            }

            items = ConfigurationContextFactory.Create().GetAll();

            foreach (ConfigurationItem configurationItem in items)
                SetCache(configurationItem.Key, configurationItem.Value);

            return items;
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

        public static string GetLocalConnectionString(string key)
        {
            string connection = CloudConfigurationManager.GetSetting(key);
            if (string.IsNullOrEmpty(connection))
                connection = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(connection))
                connection = ConfigurationManager.ConnectionStrings[key].ConnectionString;

            return connection;
        }

        public static string GetSetting(string settingKey, string defaultValue = null)
        {
            string cacheKey = settingKey;
            string value = _cache.Get(cacheKey) as string;
            if (value != null)
                return value;

            IConfigurationContext context = ConfigurationContextFactory.Create();
            ConfigurationItem item = context.GetItem(settingKey);
            if (item == null)
            {
                if (string.IsNullOrEmpty(defaultValue))
                    throw new Exception(string.Format("Configuration Setting '{0}' not found.", settingKey));
                else
                    return defaultValue;
            }

            SetCache(cacheKey, item.Value);

            return item.Value;
        }

        public static void RefreshCache()
        {
            Task.Factory.StartNew(() =>
            {
                foreach (KeyValuePair<string, object> item in _cache)
                    _cache.Remove(item.Key);
            });
        }

        public static void SaveSetting(string key, string value)
        {
            IConfigurationContext context = ConfigurationContextFactory.Create();
            context.SaveItem(key, value);
            SetCache(key, value);
        }

        private static void SetCache(string key, object value)
        {
            DateTimeOffset dateTime = DateTimeOffset.Now.AddMinutes(15);
            _cache.Set(key, value, new CacheItemPolicy { AbsoluteExpiration = dateTime });
        }
    }
}