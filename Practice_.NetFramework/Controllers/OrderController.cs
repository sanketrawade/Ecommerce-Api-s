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
    public class OrderController : BaseController
    {
        #region Order
        [HttpPost]
        [Route("api/genord")]
        public IHttpActionResult GenrateOrder()
        {
            try
            {
                OrderManager.GenerateOrder();
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/gtorddtil")]
        public IHttpActionResult GetOrderDetails([FromBody] SearchViewModel searchViewModel)
        {
            try
            {
                return Ok(OrderManager.GetOrderDetails(searchViewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/dlorddet")]
        public IHttpActionResult DeleteOrderDetails([FromBody] IdViewModel idViewModel)
        {
            try
            {
                OrderManager.DeleteOrderDetails(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/gtorddtils")]
        public IHttpActionResult GetOrderDetailsByCustomerID([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(OrderManager.GetOrderDetailsByCustomerID(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
        #endregion
        #region OrderStatus
        
        [HttpPost]
        [Route("api/gtordsttus")]
        public IHttpActionResult GetOrderStatus([FromBody] SearchViewModel searchViewModel)
        {
            try
            {
                return Ok(OrderManager.GetOrderStatus(searchViewModel));
            }
            catch(Exception exception)
            {
                return InternalServerError(exception);
            }
        }
        [HttpPost]
        [Route("api/gtordsttusid")]
        public IHttpActionResult GetOrderStatusById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(OrderManager.GetOrderStatus(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/gtordsttusordid")]
        public IHttpActionResult GetOrderStatusByOrderId([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(OrderManager.GetOrderStatusByOrderId(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }



        [HttpPost]
        [Route("api/adordsttus")]
        public IHttpActionResult AddOrderStatus([FromBody] OrderStatu orderStatus)
        {
            try
            {
                return Ok(OrderManager.AddOrderStatus(orderStatus));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/updtordsttus")]
        public IHttpActionResult UpdateOrderStatus([FromBody] OrderStatu orderStatus)
        {
            try
            {
                return Ok(OrderManager.UpdateOrderStatus(orderStatus));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/updtordsttusord")]
        public IHttpActionResult UpdateOrderStatusOfOrder([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(OrderManager.UpdateOrderStatusOfOrder(idViewModel.Id,idViewModel.status));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/delordsttus")]
        public IHttpActionResult DeleteOrderStatus([FromBody] IdViewModel idViewModel)
        {
            try
            {
                OrderManager.DeleteOrderStatus(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        #endregion
    }
}
