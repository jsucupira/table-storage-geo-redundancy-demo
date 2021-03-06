﻿using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataAccess.CustomersAts
{
    [Serializable]
    public class CustomerAtsEntity : TableEntity
    {
        public CustomerAtsEntity()
        {
            PartitionKey = CustomerAtsContext.PARTITION_KEY;
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}