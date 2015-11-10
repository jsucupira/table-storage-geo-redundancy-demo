using System;
using Core.Configuration;
using Model.Customer;

namespace Business
{
    public static class CustomerUpdator
    {
        public static void Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new ApplicationException("Customer Id cannot be null");

            CustomerContextFactory.Create().Delete(customerId);
        }

        public static Customer Save(Customer customer)
        {
            if (customer == null)
                throw new ApplicationException("Customer object cannot be null");

            if (string.IsNullOrEmpty(customer.Email))
                throw new ApplicationException("Email cannot be null");

            if (string.IsNullOrEmpty(customer.FirstName))
                throw new ApplicationException("First name cannot be null");

            if (string.IsNullOrEmpty(customer.LastName))
                throw new ApplicationException("Last name cannot be null");

            customer.CustomerId = Guid.NewGuid().ToString();

            bool result = CustomerContextFactory.Create().Save(customer);
            if (!result)
                throw new ApplicationException("Unable to save the customer");

            return customer;
        }
    }
}