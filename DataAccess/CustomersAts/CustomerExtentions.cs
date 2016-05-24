using Model.Customer;

namespace DataAccess.CustomersAts
{
    public static class CustomerExtensions
    {
        public static CustomerAtsEntity Map(this Customer model)
        {
            if (model == null) return null;
            return new CustomerAtsEntity
            {
                RowKey = model.CustomerId,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };
        }

        public static Customer Map(this CustomerAtsEntity entity)
        {
            if (entity == null) return null;
            return new Customer
            {
                CustomerId = entity.RowKey,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
            };
        }
    }
}