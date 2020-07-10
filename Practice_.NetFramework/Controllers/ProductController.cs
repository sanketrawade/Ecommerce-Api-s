using Model.Data;
using Model.Manager;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Practice_.NetFramework.Controllers
{
  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : BaseController
    {
        // GET: Product
        [HttpGet]
        [Route("api/gtproduct")]
        public IHttpActionResult GetProducts()
        {
            try
            {
                return Ok(ProductManager.GetProducts());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/gtproductlist")]
        public IHttpActionResult GetProductList([FromBody] SearchViewModel searchViewModel)
        {
            try
            {
                return Ok(ProductManager.GetProductList(searchViewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdtid")]
        public IHttpActionResult GetProductById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductManager.GetProductById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet]
        [Route("api/gtprwiord")]
        public IHttpActionResult GetProductWiseOrder()
        {
            try
            {
                return Ok(ProductManager.GetProductWiseOrder());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }



        [HttpPost]
        [Route("api/getprdtbyctid")]
        public IHttpActionResult GetProductByCateId([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductManager.GetProductByCategory(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/updtprct")]
        public IHttpActionResult UpdateProduct()
        {
            try
            {
                return Ok(ProductManager.UpdateProduct());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpGet]
        [Route("api/getprdctbyord")]
        public IHttpActionResult GetProductByOrder()
        {
            try
            {
                return Ok(ProductManager.GetProductsByOrder());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpGet]
        [Route("api/getprdctbygrp")]
        public IHttpActionResult GetProductByGroup()
        {
            try
            {
                return Ok(ProductManager.GetProductsByGroup());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /*[HttpGet]
        [Route("api/getprdctbybskt")]
        public IHttpActionResult GetProductByBasket()
        {
            try
            {
                return Ok(ProductManager.GetProductByBasket());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }*/

        [HttpPost]
        [Route("api/adprct")]
        public IHttpActionResult AddProduct()
        {
            try
            {
                return Ok(ProductManager.AddProduct());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/deltprdct")]
        public IHttpActionResult DeleteProduct([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductManager.DeleteProduct(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/deltprdqen")]
        public IHttpActionResult DeleteProductQuentity([FromBody] IdViewModel idViewModel)
        {
            try
            {
                ProductManager.DeleteProductQuentity(idViewModel.ProductId,idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}