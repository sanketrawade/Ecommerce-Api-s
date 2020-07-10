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
    public class ProductSubCategoryController : BaseController
    {
        [HttpGet]
        [Route("api/getprdsubcat")]
        public IHttpActionResult GetProductSubCategories()
        {
            try
            {
                return Ok(ProductSubCategoryManager.GetProducSubtCategories());
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdsubcatlist")]
        public IHttpActionResult GetProductCategoryList([FromBody] SearchViewModel viewModel)
        {
            try
            {
                return Ok(ProductSubCategoryManager.GetProductCategoryList(viewModel));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/getprdsubcattid")]
        public IHttpActionResult GetProductSubCategoryById([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductSubCategoryManager.GetProductSubCategoryById(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/getprdsubbycattid")]
        public IHttpActionResult GetSubCategoryByCategoryId([FromBody] IdViewModel idViewModel)
        {
            try
            {
                return Ok(ProductSubCategoryManager.GetSubcategoryByCategory(idViewModel.Id));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }


        [HttpPost]
        [Route("api/updprdsubcatt")]
        public IHttpActionResult UpdateProductCategory([FromBody] ProductSubCategory productSubCategory)
        {
            try
            {
                return Ok(ProductSubCategoryManager.UpdateProductSubCategory(productSubCategory));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/addprdsubcat")]
        public IHttpActionResult AddProductSubCategory([FromBody] ProductSubCategory productSubCategory)
        {
            try
            {
                return Ok(ProductSubCategoryManager.AddProductSubCategory(productSubCategory));
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPost]
        [Route("api/deltprdsubcat")]
        public IHttpActionResult DeleteProductSubCategory([FromBody] IdViewModel idViewModel)
        {
            try
            {
                ProductSubCategoryManager.DeleteProductSubCategory(idViewModel.Id);
                return Ok();
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
    }
}
