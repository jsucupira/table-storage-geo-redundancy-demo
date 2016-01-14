using System;
using Azure.TableStorage.Redundancy;
using DataAccess.CustomersAts;
using Model.Customer;
using Newtonsoft.Json;

namespace Shared
{
    public static class ActionStrategy
    {
        public static Action Create(this TransactionLog transactionLogMessage, string connectionString)
        {
            switch (transactionLogMessage.Type)
            {
                case nameof(CustomerAtsEntity):
                    return transactionLogMessage.Create<CustomerAtsEntity>(connectionString);
            }
            return default(Action);
        }
    }
}