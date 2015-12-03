using System.Collections.Generic;
using System.Web.Http;
using Business;
using Model.Customer;

namespace WebServicesRole.Controllers
{
    public class CustomerController : ApiController
    {
        [Route("customers")]
        [HttpPost]
        public Customer CreateUser([FromBody] Customer customer)
        {
            return CustomerUpdator.Create(customer);
        }

        [Route("customers")]
        [HttpGet]
        public List<Customer> FindAll()
        {
            return CustomerSelector.FindAll();
        }

        [Route("customers/{customerId}")]
        [HttpGet]
        public Customer Get([FromUri] string customerId)
        {
            return CustomerSelector.Get(customerId);
        }

        [Route("customers/{customerId}")]
        [HttpPut]
        public void UpdateUser([FromUri] string customerId, [FromBody] Customer customer)
        {
            CustomerUpdator.Update(customerId, customer);
        }

        [Route("customers/{customerId}")]
        [HttpDelete]
        public void DeleteUser([FromUri] string customerId)
        {
            CustomerUpdator.Delete(customerId);
        }
    }
}