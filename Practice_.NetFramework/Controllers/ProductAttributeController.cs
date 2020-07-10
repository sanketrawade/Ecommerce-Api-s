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
    public class ProductAttributeController : BaseController
    {

        [HttpGet]
        [Route("api/getprdatr")]
        public IHttpActionResult GetProductAttribute()
        {
            try
            {
                return Ok(ProductAttributeManager.GetProductAttribute());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdatrlist")]
        public IHttpActionResult GetProductAttributeList([FromBody] SearchViewModel viewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.GetProductAttributeList(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        
        [HttpPost]
        [Route("api/gtatrbubyid")]
        public IHttpActionResult GetProductAttributeById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.GetProductAtrributeById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/getatroptbyid")]
        public IHttpActionResult GetProductAttributeOptionById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.GetProductAtrributeOptionById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/gtprdtatrbysbct")]
        public IHttpActionResult GetProductAttributeBySubCategoryId([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.GetProductAtrributeBySubCategory(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/gtoptbyid")]
        public IHttpActionResult GetOptionsByAttributeId([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.GetOptionByID(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/addprdatrt")]
        public IHttpActionResult AddProductAttribute([FromBody] ProductAttributeViewModel viewModel)
        {
            try
            {
                return Ok(ProductAttributeManager.AddProductAttribute(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/updprdatr")]
        public IHttpActionResult UpdateProductCategory([FromBody] ProductAttributeViewModel productAttribute)
        {
            try
            {
                return Ok(ProductAttributeManager.UpdateProductAttribute(productAttribute));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }



        [HttpPost]
        [Route("api/deltprdatr")]
        public IHttpActionResult DeleteProductAttribute([FromBody] IdViewModel idViewModel)
        {
            try
            {
                ProductAttributeManager.DeleteProductAttribute(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/deloptbyid")]
        public IHttpActionResult DeleteAttributeOption([FromBody] IdViewModel idViewModel)
        {
            try
            {
                ProductAttributeManager.DeleteProductOptionById(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
