using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Azure.TableStorage.Redundancy;
using Core.Configuration;
using Core.Extensibility;
using Model.Customer;

namespace DataAccess.CustomersAts
{
    [MefExport(typeof (ICustomerContext), "CustomerAtsContext")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class CustomerAtsContext : ICustomerContext
    {
        internal const string PARTITION_KEY = "Customer";
        private RedundantTableStorage<CustomerAtsEntity> _privateTable; 
        private RedundantTableStorage<CustomerAtsEntity> _azureTable
        {
            get
            {
                if (_privateTable == null)
                {
                    string storageAccount = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
                    string serviceBus = ConfigurationsSelector.GetSetting("RedundancyServiceBusConnection");
                    string serviceBusQueue = ConfigurationsSelector.GetSetting("Customer.Queue");
                    string archiveStorage = ConfigurationsSelector.GetLocalConnectionString("StorageArchiveAccount");
                    _privateTable = new RedundantTableStorage<CustomerAtsEntity>(storageAccount, "Customer", _redundant, serviceBus, serviceBusQueue, archiveStorage);
                }
                return _privateTable;
            }
        }
        private bool _redundant;

        public void Save(Customer customer)
        {
            _azureTable.Upsert(customer.Map());
        }

        public Customer Get(string customerId)
        {
            return _azureTable.FindBy(PARTITION_KEY, customerId).Map();
        }

        public void Delete(string customerId)
        {
            _azureTable.Delete(PARTITION_KEY, customerId);
        }

        public List<Customer> FindAll()
        {
            return _azureTable.FindByPartitionKey(PARTITION_KEY).Select(CustomerExtentions.Map).ToList();
        }

        public void Redundant()
        {
            _redundant = true;
        }
    }
}