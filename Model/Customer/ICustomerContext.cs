using System.Collections.Generic;

namespace Model.Customer
{
    public interface ICustomerContext
    {
        void Save(Customer customer);
        void Delete(string customerId);
        Customer Get(string customerId);
        List<Customer> FindAll();
        void Redundant();
    }
}
