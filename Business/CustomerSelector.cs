using System;
using System.Collections.Generic;
using Core.Configuration;
using Model.Customer;

namespace Business
{
    public static class CustomerSelector
    {
        public static List<Customer> FindAll()
        {
            return CustomerContextFactory.Create().FindAll();
        }

        public static Customer Get(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new ApplicationException("Customer Id cannot be null");

            return CustomerContextFactory.Create().Get(customerId);
        }
    }
}