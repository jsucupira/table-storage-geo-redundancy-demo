using System;
using Azure.TableStorage.Redundancy;
using DataAccess.CustomersAts;
using Model.Customer;
using Newtonsoft.Json;

namespace Shared
{
    public static class ReplicationStrategy
    {
        public static Action Create(TransactionLog transactionLogMessage)
        {
            switch (transactionLogMessage.Type)
            {
                case nameof(CustomerAtsEntity):
                    ICustomerContext context = CustomerContextFactory.CreateSimple();
                    var message = JsonConvert.DeserializeObject<CustomerAtsEntity>(transactionLogMessage.Object).Map();
                    message.ReferenceId = transactionLogMessage.TransactionId;
                    string actionType = transactionLogMessage.Action;
                    if (actionType.Equals("UPSERT", StringComparison.OrdinalIgnoreCase) || actionType.Equals("INSERT", StringComparison.OrdinalIgnoreCase))
                        return () => context.Save(message);
                    if (actionType.Equals("delete", StringComparison.OrdinalIgnoreCase))
                        return () => context.Delete(message.CustomerId);
                    break;

                default:
                    break;
            }
            return default(Action);
        }
    }
}