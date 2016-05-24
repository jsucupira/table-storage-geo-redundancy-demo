using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Azure.TableStorage.Utilities.Tables;
using Core.Configuration;
using Core.Extensibility;

namespace DataAccess.ConfigurationAts
{
    [Export(typeof(IConfigurationContext))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ConfigurationContext : IConfigurationContext
    {
        private const string PARTITION_KEY = "Configuration";
        private readonly IAzureTableUtility _azureTable;

        public ConfigurationContext()
        {
            _azureTable = MefBase.Resolve<IAzureTableUtility>();
            _azureTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            _azureTable.TableName = "Configuration";
            _azureTable.CreateTable();
        }

        public ConfigurationItem GetItem(string key)
        {
            ConfigurationItem configurationItem = _azureTable.FindBy<ConfigurationAts>(PARTITION_KEY, key).Map();
            return configurationItem;
        }

        public List<ConfigurationItem> GetAll()
        {
            return _azureTable.FindByPartitionKey<ConfigurationAts>(PARTITION_KEY).Select(ConfigurationExtensions.Map).ToList();
        }

        public void SaveItem(string key, string value)
        {
            _azureTable.Upset<ConfigurationAts>(new ConfigurationAts { RowKey = key, Value = value });
        }
    }
}