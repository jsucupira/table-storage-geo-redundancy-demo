using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Business;
using Core.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Model.Archiver;

namespace EastWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);
        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        private QueueClient _client;

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = ConfigurationsSelector.GetSetting("WestServiceBusConnection");
            string queueName = ConfigurationsSelector.GetSetting("Customer.Queue");
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            QueueDescription queueDescription = new QueueDescription(queueName)
            {
                MaxSizeInMegabytes = 1024,
                DefaultMessageTimeToLive = TimeSpan.FromDays(15),
                EnablePartitioning = true,
                EnableDeadLetteringOnMessageExpiration = true,
                LockDuration = TimeSpan.FromMinutes(5)
            };

            if (!namespaceManager.QueueExists(queueName))
                namespaceManager.CreateQueue(queueDescription);

            // Initialize the connection to Service Bus Queue
            _client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            _client.Close();
            _completedEvent.Set();
            base.OnStop();
        }

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            _client.OnMessage(receivedMessage =>
            {
                try
                {
                    // Process the message
                    Trace.WriteLine("Processing Service Bus message: " + receivedMessage.MessageId.ToString());
                    Archive message = receivedMessage.GetBody<Archive>();
                    if (message != null)
                    {
                        Action decideStrategy = ReplicationStrategy.Create(message);
                        decideStrategy?.Invoke();
                        receivedMessage.Complete();
                    }
                }
                catch
                {
                    //Need to figure out how to handle errors within Service bus.
                    receivedMessage.Abandon();
                }
            });

            _completedEvent.WaitOne();
        }
    }
}