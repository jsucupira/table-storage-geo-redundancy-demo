using Microsoft.WindowsAzure.Storage.Table;

namespace DataAccess
{
    public class ConfigurationAts : TableEntity
    {
        public string Value { get; set; }

        public ConfigurationAts()
        {
            PartitionKey = "Configuration";
        }
    }
}