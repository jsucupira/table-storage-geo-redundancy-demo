Azure Table Storage Multi-Region
===========================

The purpose of this application is for me to come up with a pattern to do multi-region writes to table storage in case of a fail-over scenario. In theory, this would make the application resilient in case an Azure data center went down.

----------

Infrastructure
----------------
The following are the required infrastructure in order to make this implementation work:

 - Cloud Services
	 - One for east data center REST services
	 - One for west data center REST services
	 - One for east data center Service Bus worker role
	 - One for west data center Service Bus worker role
 - Traffic Manager
	 - This will be setup in fail-over mode
	 - This coordinates traffic between the REST services
 - Storage Accounts
	 - One for east
	 - One for west


Application Layout
----------------------
I believe most of the application is self-explanatory, so I will just highlight some small things.

 - All application configurations are stored inside a table in Table Storage
	 - The table is called Configuration
	 - You can find the implementation inside Core.Configuration namespace
 - Dependency Injection
	 - I am using MEF for my IOC container
	 - The implementation for all the data access are inside the project DataAccess (obviously)
 - Azure Helper Library
	 - I am using a dll that contains my Azure Storage helper
	 - The source code for this dll is here: https://github.com/jsucupira/azure-blob-utilities
 - Unit Test
	 - The unit test uses my Azure Storage Helper Mock classes
	 - The Mock classes does all the processing in memory instead of communicating with Table Storage
 -  Multi-Writes
	 - Currently, if you look at the class CustomerAtsRedundancyContext under DataAccess.CustomersAts, you will see the implementation for writing to multiple places. This is the flow:
		 - First, it writes to the same data center table storage
		 - Second, it writes to a table called Archive. This table contains all of the transactions in order in case you need to reply to those transactions
		 - Third, it sends a message to service bus for that record to be added to the other region's table storage
	 - You may notice that there is a field called referenceId in the Model saved into table storage. The purpose of this field is to link a record between the archive table and the customer table
		 - You may also notice that the original record doesn't have any value on the field referenceId, only records that were added via Service Bus 


> **Application Configuration Required:**
<i>The following configurations are required in order for the application to work</i>
> - Customer.Queue
> - ServiceBusConnection
> - Customer.DataAccess (It has a default)
> - RedundancyServiceBusConnection