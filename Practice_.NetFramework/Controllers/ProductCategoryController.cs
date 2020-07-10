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
    public class ProductCategoryController : BaseController
    {
        [HttpGet]
        [Route("api/getprdcat")]
        public IHttpActionResult GetProductCategories()
        {
            try
            {
                return Ok(ProductCateogoryManager.GetProductCategories());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdcatlist")]
        public IHttpActionResult GetProductCategoryList([FromBody] SearchViewModel viewModel)
        {
            try
            {
                return Ok(ProductCateogoryManager.GetProductCategoryList(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdcattid")]
        public IHttpActionResult GetProductCategoryById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductCateogoryManager.GetProductCategoryById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/updprdcatt")]
        public IHttpActionResult UpdateProductCategory([FromBody] ProductCateogory productCategory)
        {
            try
            {
                return Ok(ProductCateogoryManager.UpdateProductCategory(productCategory));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/addprdcat")]
        public IHttpActionResult AddProductCategory([FromBody] ProductCateogory productCategory)
        {
            try
            {
                return Ok(ProductCateogoryManager.AddProductCategory(productCategory));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/deltprdcat")]
        public IHttpActionResult DeleteProductCategory([FromBody] IdViewModel idViewModel)
        {
            try
            {
                ProductCateogoryManager.DeleteProductCategory(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
