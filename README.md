Azure Table Storage Multi-Region
===========================

The purpose of this application is to demostrate the usage of the library https://github.com/jsucupira/table-storage-geo-redundancy.

----------

Infrastructure
----------------
The following are the required infrastructure in order to make this implementation work:

 - 4 Cloud Services
	 - One cloud services in the east data center to host the REST services
	 - One cloud services in the west data center to host the REST services
	 - One cloud services in the east data center to host the Service Bus worker role
	 - One cloud services in the west data center to host the Service Bus worker role
 - Traffic Manager
	 - The traffic manager will coordinate the traffic between the REST services
	 - The traffic manager will be setup in a fail-over mode
 - Storage Accounts
	 - One storage in east
	 - One storage in west

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
	 - Currently I am using the library Azure.TableStorage.Redundancy to abstract the multi-write. This is the flow it follows:
		 - First, it writes to the same data center table storage
		 - Second, it writes to a table called TransactionLog. This table contains all of the transactions in order in case you need to reply to those transactions
		 - Third, it sends a message to service bus for that record to be added to the other region's table storage

> **Application Configuration Required:**
<i>The following configurations are required in order for the application to work</i>
> - Customer.Queue
> - ServiceBusConnection
> - RedundancyServiceBusConnection (the value will be the secondary(other's region) service bus connection string)