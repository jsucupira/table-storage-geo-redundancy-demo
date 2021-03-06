﻿using System;
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

        public static Customer Create(Customer customer)
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

            CustomerContextFactory.Create().Save(customer);

            return customer;
        }

        public static Customer Update(string customerId, Customer customer)
        {
            if (customer == null)
                throw new ApplicationException("Customer object cannot be null");

            Guid id;

            if (!Guid.TryParse(customerId, out id))
                throw new ApplicationException("The customer id is not valid");

            if (string.IsNullOrEmpty(customer.Email))
                throw new ApplicationException("Email cannot be null");

            if (string.IsNullOrEmpty(customer.FirstName))
                throw new ApplicationException("First name cannot be null");

            if (string.IsNullOrEmpty(customer.LastName))
                throw new ApplicationException("Last name cannot be null");

            customer.CustomerId = customerId;

            CustomerContextFactory.Create().Save(customer);

            return customer;
        }
    }
}