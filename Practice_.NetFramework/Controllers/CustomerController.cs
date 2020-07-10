using Model.Data;
using Model.Manager;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Practice_.NetFramework.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerController : BaseController
    {
        // GET api/<controller>
        [HttpPost]
        [Route("api/lgnusr")]
        public IHttpActionResult ValidateUser([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                return Ok(CustomerManager.ValidateCustomer(loginViewModel.username, loginViewModel.password));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/regcust")]
        public IHttpActionResult RegisterCustomer()
        {
            try
            {
                return Ok(CustomerManager.RegisterCustomer());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/sndeml")]
        public IHttpActionResult SendEmail([FromBody] IdViewModel viewModel)
        {
            try
            {
                return Ok(CustomerManager.SendEmail(viewModel.emailId, viewModel.password,viewModel.registrationFlag));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/chngpas")]
        public IHttpActionResult ChangePassword([FromBody] PasswordViewModel viewModel)
        {
            try
            {
                return Ok(CustomerManager.ChangePassword(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

       /* [HttpPost]
        [Route("api/chnpasran")]
        public IHttpActionResult ChangePasswordToRandom([FromBody] PasswordViewModel viewModel)
        {
            try
            {
                return Ok(CustomerManager.ChangePasswordToRandom(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }*/


        [HttpPost] 
        [Route("api/uptcustprof")]
        public IHttpActionResult UpdateCustomer()
        {
            try
            {
                return Ok(CustomerManager.UpdateCustomerProfile());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet]
        [Route("api/gtcustmr")]
        public IHttpActionResult GetCustomers()
        {
            try
            {
                return Ok(CustomerManager.GetCustomers());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getcstmerid")]
        public IHttpActionResult GetCustomerById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(CustomerManager.GetCustomersById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/addcstmer")]
        public IHttpActionResult AddCustomer(Customer customer)
        {
            try
            {
                CustomerManager.AddCustomer(customer);
                return Ok(customer);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/dltcstmer")]
        public IHttpActionResult DeleteCustomer(int id)
        {
            try
            {
                CustomerManager.DeleteCustomer(id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/updtlstlgin")]
        public IHttpActionResult UpdateLastLogin(IdViewModel viewModel)
        {
            try
            {
                return Ok(CustomerManager.UpdateLastLogin(viewModel.emailId));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

    }
}