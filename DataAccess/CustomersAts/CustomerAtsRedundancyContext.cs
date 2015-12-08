using System.Collections.Generic;
using System.ComponentModel.Composition;
using AzureUtilities.Service_Bus;
using Core.Configuration;
using Core.Extensibility;
using Model.Customer;
using Model.Transaction;
using Newtonsoft.Json;

namespace DataAccess.CustomersAts
{
    [MefExport(typeof(ICustomerContext), "CustomerAtsRedundancyContext")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerAtsRedundancyContext : ICustomerContext
    {
        private readonly ITransactionLogContext _transactionLogContext;
        private readonly ICustomerContext _customerContext;
        private readonly IServiceBusContext _serviceBusContext;

        public CustomerAtsRedundancyContext()
        {
            _customerContext = new CustomerAtsContext();
            _transactionLogContext = TransactionLogContextFactory.Create("CustomerAtsTransactionLogContext");
            _serviceBusContext = MefBase.Resolve<IServiceBusContext>();
            _serviceBusContext.ConnectionString = ConfigurationsSelector.GetSetting("RedundancyServiceBusConnection");
        }

        public bool Save(Customer customer)
        {
            bool result = _customerContext.Save(customer);
            if (result)
            {
                TransactionLog archivedMessage = _transactionLogContext.Save(new TransactionLog
                {
                    Action = "Save",
                    Object = JsonConvert.SerializeObject(customer),
                    Type = nameof(Customer)
                });
                _serviceBusContext.AddToQueue(ConfigurationsSelector.GetSetting("Customer.Queue"), archivedMessage);
            }
            return result;
        }

        public Customer Get(string customerId)
        {
            return _customerContext.Get(customerId);
        }

        public bool Delete(string customerId)
        {
            Customer customer = Get(customerId);
            bool result = _customerContext.Delete(customerId);
            if (result)
            {
                TransactionLog archivedMessage = _transactionLogContext.Save(new TransactionLog
                {
                    Action = "Delete",
                    Object = JsonConvert.SerializeObject(customer),
                    Type = nameof(Customer)
                });
                _serviceBusContext.AddToQueue(ConfigurationsSelector.GetSetting("Customer.Queue"), archivedMessage);
            }
            return result;
        }

        public List<Customer> FindAll()
        {
            return _customerContext.FindAll();
        }
    }
}