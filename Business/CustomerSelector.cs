using System;
using System.Collections.Generic;
using Core.Configuration;
using Core.Exceptions;
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
                throw new InvalidValueException(nameof(customerId), customerId);

            var customer = CustomerContextFactory.Create().Get(customerId);
            if (customer == null)
                throw new NotFoundException("Customer");

            return customer;
        }
    }
}