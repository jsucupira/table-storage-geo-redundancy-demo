using System.Collections.Generic;
using Model.Archiver;

namespace Model.Customer
{
    public interface ICustomerContext
    {
        bool Save(Customer customer);
        bool Delete(string customerId);
        Customer Get(string customerId);
        List<Customer> FindAll();
    }
}
