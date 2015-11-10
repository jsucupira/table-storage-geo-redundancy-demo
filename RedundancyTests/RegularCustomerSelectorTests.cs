using AzureUtilities.Tables;
using Business;
using Core.Configuration;
using Core.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Customer;

namespace RedundancyTests
{
    [TestClass]
    public class RegularCustomerSelectorTests
    {
        [TestInitialize]
        public void Init()
        {
            MefLoader.Init();
            IAzureTableUtility azureTableUtility = MefBase.Resolve<IAzureTableUtility>();
            azureTableUtility.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            azureTableUtility.TableName = "Customer";
            azureTableUtility.DeleteTable();
        }
        
        [TestMethod]
        public void test_get_all()
        {
            for (int i = 0; i < 10; i++)
            {
                CustomerUpdator.Save(new Customer
                {
                    FirstName = "Jonathas",
                    Email = "jonathas@jsucupira.com",
                    LastName = "Sucupira"
                });
            }
            Assert.IsTrue(CustomerSelector.FindAll().Count == 10);
        }

        [TestMethod]
        public void test_get_single()
        {
            Customer customer = CustomerUpdator.Save(new Customer
            {
                FirstName = "Jonathas",
                Email = "jonathas@jsucupira.com",
                LastName = "Sucupira"
            });
            Assert.IsNotNull(CustomerSelector.Get(customer.CustomerId));
        }
    }
}