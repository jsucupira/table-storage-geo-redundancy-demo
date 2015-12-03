using System;
using AzureUtilities.Tables;
using Business;
using Core.Configuration;
using Core.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Archiver;
using Model.Customer;

namespace RedundancyTests
{
    [TestClass]
    public class ReplicatedCustomerUpdatorTests
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

            ConfigurationsSelector.SaveSetting("ServiceBusConnection", "Endpoint=sb://random.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=awdawdAWDAWDAWdwadad==");
            ConfigurationsSelector.SaveSetting("Customer.Queue", "blah");
            ConfigurationsSelector.SaveSetting("Customer.DataAccess", "CustomerAtsRedundancyContext");
        }

        [TestMethod]
        public void test_delete()
        {
            Customer customer = CustomerUpdator.Create(new Customer
            {
                FirstName = "Jonathas",
                Email = "jonathas@jsucupira.com",
                LastName = "Sucupira"
            });

            CustomerUpdator.Delete(customer.CustomerId);
        }

        [TestMethod]
        public void test_save()
        {
            for (int i = 0; i < 10; i++)
            {
                CustomerUpdator.Create(new Customer
                {
                    Email = i.ToString(),
                    FirstName = i.ToString(),
                    LastName = i.ToString()
                });
            }

            Assert.IsTrue(CustomerSelector.FindAll().Count == 10);
            IArchiveContext customerArchiver = ArchiveContextFactory.Create("CustomerAtsArchiveContext");
            Assert.IsTrue(customerArchiver.FindAll().Count == 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void test_invalid_save()
        {
            CustomerUpdator.Create(new Customer());
        }
    }
}