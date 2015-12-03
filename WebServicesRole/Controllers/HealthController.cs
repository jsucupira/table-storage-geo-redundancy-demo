using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebServicesRole.Controllers
{
    public class HealthController : ApiController
    {
        [Route("health")]
        [HttpGet]
        public string Check()
        {
            return "Ok";
        }
    }
}
