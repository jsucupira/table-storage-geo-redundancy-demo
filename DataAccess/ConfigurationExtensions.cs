using Core.Configuration;

namespace DataAccess
{
    public static class ConfigurationExtensions
    {
        public static ConfigurationItem Map(this ConfigurationAts config)
        {
            return new ConfigurationItem
            {
                Key = config.RowKey,
                Value = config.Value
            };
        }
        public static ConfigurationAts Map(this ConfigurationItem config)
        {
            return new ConfigurationAts
            {
                RowKey = config.Key,
                Value = config.Value
            };
        }
    }
}