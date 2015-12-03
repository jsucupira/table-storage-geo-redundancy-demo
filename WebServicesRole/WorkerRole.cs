using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using Services.BootStrapper;

namespace WebServicesRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private IDisposable _app;

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 50;

            string sSource = "Web Services";
            string sLog = "Application";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            try
            {
                MefLoader.Initialize();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sSource, ex.Message);
                if (ex.InnerException != null)
                    EventLog.WriteEntry(sSource, ex.InnerException.Message);
            }

            RoleInstanceEndpoint endPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HttpEndPoint"];
            string baseUri = $"{endPoint.Protocol}://{endPoint.IPEndpoint}";
            _app = WebApp.Start<Startup>(new StartOptions(baseUri));
            bool result = base.OnStart();
            Trace.TraceInformation("WebServicesRole has been started");
            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WebServicesRole is stopping");

            _cancellationTokenSource.Cancel();
            _runCompleteEvent.WaitOne();

            _app?.Dispose();

            base.OnStop();

            Trace.TraceInformation("WebServicesRole has stopped");
        }

        public override void Run()
        {
            Trace.TraceInformation("WebServicesRole is running");

            try
            {
                RunAsync(_cancellationTokenSource.Token).Wait();
            }
            finally
            {
                _runCompleteEvent.Set();
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(10000);
            }
        }
    }
}