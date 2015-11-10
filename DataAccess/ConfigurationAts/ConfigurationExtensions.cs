using Core.Configuration;

namespace DataAccess.ConfigurationAts
{
    public static class ConfigurationExtensions
    {
        public static ConfigurationItem Map(this ConfigurationAts config)
        {
            if (config == null) return null;
            return new ConfigurationItem
            {
                Key = config.RowKey,
                Value = config.Value
            };
        }

        public static ConfigurationAts Map(this ConfigurationItem config)
        {
            if (config == null) return null;
            return new ConfigurationAts
            {
                RowKey = config.Key,
                Value = config.Value
            };
        }
    }
}