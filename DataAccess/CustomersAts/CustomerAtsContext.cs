using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using AzureUtilities.Tables;
using Core.Configuration;
using Core.Extensibility;
using Microsoft.WindowsAzure.Storage.Table;
using Model.Archiver;
using Model.Customer;

namespace DataAccess.CustomersAts
{
    [MefExport(typeof (ICustomerContext), "CustomerAtsContext")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class CustomerAtsContext : ICustomerContext
    {
        internal const string PARTITION_KEY = "Customer";
        private readonly IAzureTableUtility _azureTable;

        public CustomerAtsContext()
        {
            _azureTable = MefBase.Resolve<IAzureTableUtility>();
            _azureTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            _azureTable.TableName = "Customer";
            _azureTable.CreateTable();
        }

        public bool Save(Customer customer)
        {
            TableResult result = _azureTable.Upset<CustomerAtsEntity>(customer.Map());
            return result.HttpStatusCode == (int) HttpStatusCode.NoContent;
        }

        public Customer Get(string customerId)
        {
            return _azureTable.FindBy<CustomerAtsEntity>(PARTITION_KEY, customerId).Map();
        }

        public bool Delete(string customerId)
        {
            _azureTable.DeleteEntity(PARTITION_KEY, customerId);
            return true;
        }

        public List<Customer> FindAll()
        {
            return _azureTable.FindByPartitionKey<CustomerAtsEntity>(PARTITION_KEY).Select(CustomerExtentions.Map).ToList();
        }
    }
}