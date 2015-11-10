using System;
using System.Configuration;
using System.Linq;
using AzureUtilities.Tables;
using Core.Configuration;
using Core.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedundancyTests
{
    [TestClass]
    public class ConfigurationSelectorTests
    {
        [TestInitialize]
        public void Init()
        {
            MefLoader.Init();
            IAzureTableUtility azureTableUtility = MefBase.Resolve<IAzureTableUtility>();
            azureTableUtility.ConnectionString = ConfigurationsSelector.GetLocalConnectionString("StorageAccount");
            azureTableUtility.TableName = "Configuration";
            azureTableUtility.DeleteTable();
        }

        [TestMethod]
        public void test_get_all()
        {
            Assert.IsFalse(ConfigurationsSelector.GetAll().Any());
            for (int i = 0; i < 10; i++)
                ConfigurationsSelector.SaveSetting($"loop_{i}", Guid.NewGuid().ToString());
            
            Assert.IsTrue(ConfigurationsSelector.GetAll().Count == 10);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Configuration Setting 'blah' not found.")]
        public void test_get_non_existing()
        {
            ConfigurationsSelector.GetSetting("blah");
        }

        [TestMethod]
        public void test_get_boolean()
        {
            ConfigurationsSelector.SaveSetting("boolean1", "true");
            Assert.IsTrue(ConfigurationsSelector.GetBooleanSetting("boolean1"));
            ConfigurationsSelector.SaveSetting("boolean2", "1");
            Assert.IsTrue(ConfigurationsSelector.GetBooleanSetting("boolean2"));
            
            ConfigurationsSelector.SaveSetting("boolean1", "false");
            Assert.IsFalse(ConfigurationsSelector.GetBooleanSetting("boolean1"));
            ConfigurationsSelector.SaveSetting("boolean2", "0");
            Assert.IsFalse(ConfigurationsSelector.GetBooleanSetting("boolean2"));
        }

        [TestMethod]
        public void test_get_double()
        {
            ConfigurationsSelector.SaveSetting("double1", "0.5");
            Assert.AreEqual(0.5, ConfigurationsSelector.GetDoubleSetting("double1"));
        }

        [TestMethod]
        public void test_get_int()
        {
            ConfigurationsSelector.SaveSetting("int1", "3");
            Assert.AreEqual(3, ConfigurationsSelector.GetIntegerSetting("int1"));
        }

        [TestMethod]
        public void test_get_local_connection()
        {
            Assert.AreEqual(ConfigurationManager.AppSettings["StorageAccount"], ConfigurationsSelector.GetLocalConnectionString("StorageAccount"));
        }
    }
}
