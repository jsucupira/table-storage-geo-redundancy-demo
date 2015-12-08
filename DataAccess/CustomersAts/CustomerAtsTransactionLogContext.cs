using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AzureUtilities.Tables;
using Core.Configuration;
using Core.Extensibility;
using DataAccess.TransactionAts;
using Model.Transaction;

namespace DataAccess.CustomersAts
{
    [MefExport(typeof (ITransactionLogContext), "CustomerAtsTransactionLogContext")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class CustomerAtsTransactionLogContext : ITransactionLogContext
    {
        private const string PARTITION_KEY = "Customer";
        private readonly IAzureTableUtility _azureTable;

        public CustomerAtsTransactionLogContext()
        {
            _azureTable = MefBase.Resolve<IAzureTableUtility>();
            _azureTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageArchiveAccount");
            _azureTable.TableName = "TransactionLog";
            _azureTable.CreateTable();
        }

        public List<TransactionLog> FindAll()
        {
            return _azureTable.FindByPartitionKey<TransactionLogAtsEntity>(PARTITION_KEY).Select(TransactionLogExtensions.Map).ToList();
        }

        public TransactionLog Save(TransactionLog transactionLog)
        {
            TransactionLogAtsEntity atsEntity = transactionLog.Map();
            _azureTable.Insert(atsEntity);
            return atsEntity.Map();
        }
    }
}