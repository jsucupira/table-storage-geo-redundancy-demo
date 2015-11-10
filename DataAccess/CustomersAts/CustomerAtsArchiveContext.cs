using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AzureUtilities.Tables;
using Core.Configuration;
using Core.Extensibility;
using DataAccess.ArchiverAts;
using Model.Archiver;

namespace DataAccess.CustomersAts
{
    [MefExport(typeof (IArchiveContext), "CustomerAtsArchiveContext")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    internal class CustomerAtsArchiveContext : IArchiveContext
    {
        private const string PARTITION_KEY = "Customer";
        private readonly IAzureTableUtility _azureTable;

        public CustomerAtsArchiveContext()
        {
            _azureTable = MefBase.Resolve<IAzureTableUtility>();
            _azureTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageArchiveAccount");
            _azureTable.TableName = "CustomerArchive";
            _azureTable.CreateTable();
        }

        public List<Archive> FindAll()
        {
            return _azureTable.FindByPartitionKey<ArchiveAtsEntity>(PARTITION_KEY).Select(ArchiveExtensions.Map).ToList();
        }

        public Archive Save(Archive archive)
        {
            ArchiveAtsEntity atsEntity = archive.Map();
            _azureTable.Insert(atsEntity);
            return atsEntity.Map();
        }
    }
}