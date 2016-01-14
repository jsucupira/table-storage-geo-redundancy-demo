using System;

namespace Model.Customer
{
    [Serializable]
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        //if this value is null in the table means that it was added in the primary region
    }
}