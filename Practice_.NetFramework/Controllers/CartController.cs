using Model.Manager;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Practice_.NetFramework.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CartController : BaseController
    {
        [HttpPost]
        [Route("api/gtcrt")]
        public IHttpActionResult GetCartItem([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(CartManager.GetCartItems(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/gtcrtcnt")]
        public IHttpActionResult GetCartCount([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(CartManager.GetCartItemCount(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet]
        [Route("api/gttxval")]
        public IHttpActionResult GetTaxValues()
        {
            try
            {
                return Ok(CartManager.GetTax());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/adtocrt")]
        public IHttpActionResult AddtoCart([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(CartManager.AddItemtoCart(idViewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/rmsngitm")]
        public IHttpActionResult RemoveSingleCartItem([FromBody] IdViewModel idViewModel)
        {
            try
            {
                CartManager.RemoveSingleItemFromCart(idViewModel.Id, idViewModel.ProductId);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/rmvcrtitems")]
        public IHttpActionResult RemoveCartItem([FromBody] IdViewModel idViewModel)
        {
            try
            {
                CartManager.RemoveCartItem(idViewModel.Id, idViewModel.ProductId);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/rmvallcrtitem")]
        public IHttpActionResult RemoveAllCartItem([FromBody] IdViewModel idViewModel)
        {
            try
            {
                CartManager.RemoveAllBCartItems(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
