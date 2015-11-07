using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using AzureUtilities.Tables;
using Core.Configuration;
using Core.Extensibility;

namespace DataAccess
{
    [Export(typeof (IConfigurationContext))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ConfigurationContext : IConfigurationContext
    {
        private readonly IAzureTableUtility _azureTable;
        private const string PARTITION_KEY = "Configuration";

        public ConfigurationContext()
        {
            _azureTable = MefBase.Resolve<IAzureTableUtility>();
            _azureTable.ConnectionString = ConfigurationManager.AppSettings["BlobConnectionString"];
            _azureTable.TableName = "Configuration";
            _azureTable.CreateTable();
        }

        public ConfigurationItem GetItem(string key)
        {
            return _azureTable.FindBy<ConfigurationAts>(PARTITION_KEY, key).Map();
        }

        public ConfigurationItems GetAll()
        {
            return (ConfigurationItems) _azureTable.FindByPartitionKey<ConfigurationAts>(PARTITION_KEY).Select(ConfigurationExtensions.Map).ToList();
        }

        public void SaveItem(string key, string value)
        {
            _azureTable.Upset<ConfigurationAts>(new ConfigurationAts() {RowKey = key, Value = value});
        }
    }
}