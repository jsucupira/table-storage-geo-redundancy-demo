using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace WebServicesRole
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Services.Replace(typeof(IHttpActionInvoker), new ControllerActionInvoker());
            config.MapHttpAttributeRoutes();
            ConfigureApi(config);
            app.UseWebApi(config);
            config.EnsureInitialized();
        }

        private static void ConfigureApi(HttpConfiguration config)
        {
            int index = config.Formatters.IndexOf(config.Formatters.JsonFormatter);
            config.Formatters[index] = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            };
        }
    }
}