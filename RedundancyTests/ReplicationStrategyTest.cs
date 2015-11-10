using System;
using AzureUtilities.Tables;
using Business;
using Core.Configuration;
using Core.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Archiver;
using Model.Customer;
using Newtonsoft.Json;

namespace RedundancyTests
{
    [TestClass]
    public class ReplicationStrategyTest
    {
        [TestInitialize]
        public void Init()
        {
            MefLoader.Init();
            IAzureTableUtility customerTable = MefBase.Resolve<IAzureTableUtility>();
            customerTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            customerTable.TableName = "Customer";
            customerTable.DeleteTable();

            IAzureTableUtility archiveTable = MefBase.Resolve<IAzureTableUtility>();
            archiveTable.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            archiveTable.TableName = "CustomerArchive";
            archiveTable.DeleteTable();
            ConfigurationsSelector.SaveSetting("Customer.DataAccess", "CustomerAtsContext");
        }

        [TestMethod]
        public void test_strategy()
        {
            IArchiveContext archiveContext = ArchiveContextFactory.Create("CustomerAtsArchiveContext");
            for (int i = 0; i < 5; i++)
            {
                Customer customer = new Customer
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    FirstName = "Jonathas" + i,
                    Email = "jonathas@jsucupira.com",
                    LastName = "Sucupira"
                };
                archiveContext.Save(new Archive
                {
                    Action = "Save",
                    Object = JsonConvert.SerializeObject(customer),
                    Type = nameof(Customer)
                });
            }

            foreach (Archive item in archiveContext.FindAll())
            {
                Action action = ReplicationStrategy.Create(item);
                action();
            }

            Assert.IsTrue(CustomerContextFactory.Create().FindAll().Count == 5);
        }
    }
}