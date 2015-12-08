using System;
using Model.Transaction;

namespace Model.Customer
{
    [Serializable]
    public class Customer : RedundancyModel
    {
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}