using System;
using Model.Archiver;
using Model.Customer;
using Newtonsoft.Json;

namespace Business
{
    public static class ReplicationStrategy
    {
        public static Action Create(Archive archiveMessage)
        {
            switch (archiveMessage.Type)
            {
                case nameof(Customer):
                    ICustomerContext context = CustomerContextFactory.CreateSimple();
                    Customer message = JsonConvert.DeserializeObject<Customer>(archiveMessage.Object);
                    message.ReferenceId = archiveMessage.ArchiveId;
                    string actionType = archiveMessage.Action;
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