using System;
using Model.Customer;
using Model.Transaction;
using Newtonsoft.Json;

namespace Business
{
    public static class ReplicationStrategy
    {
        public static Action Create(TransactionLog transactionLogMessage)
        {
            switch (transactionLogMessage.Type)
            {
                case nameof(Customer):
                    ICustomerContext context = CustomerContextFactory.CreateSimple();
                    Customer message = JsonConvert.DeserializeObject<Customer>(transactionLogMessage.Object);
                    message.ReferenceId = transactionLogMessage.TransactionId;
                    string actionType = transactionLogMessage.Action;
                    if (actionType.Equals("save", StringComparison.OrdinalIgnoreCase))
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