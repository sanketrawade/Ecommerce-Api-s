using Model.Data;
using Model.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.Manager
{
    public class ProductManager
    {
        #region Product
        public static List<GetProduct_Result> GetProducts()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<GetProduct_Result> products = entities.GetProduct().ToList();
                return products;
            }
        }

        public static ResponseViewModel<PageViewModel<GetProductList_Result>> GetProductList(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<GetProductList_Result>> ViewModel = new ResponseViewModel<PageViewModel<GetProductList_Result>>();
                List<GetProductList_Result> getProductResult = new List<GetProductList_Result>();
                PageViewModel<GetProductList_Result> pageViewModel = new PageViewModel<GetProductList_Result>();
                getProductResult = entities.GetProductList(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == ""?null:searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
                pageViewModel.Data = getProductResult;
                pageViewModel.totalRecords = getProductResult.Count();
                if(((pageViewModel.totalRecords % searchViewModel.pageSize) == 0))
                {
                    if(pageViewModel.totalRecords < searchViewModel.pageSize)
                    {
                        pageViewModel.totalPage = 1;
                    }
                    else
                    {
                        pageViewModel.totalPage = (pageViewModel.totalRecords / searchViewModel.pageSize);
                    }
                }
                else
                {
                    pageViewModel.totalPage = (pageViewModel.totalRecords / 10) + 1;
                }
                ViewModel.Data = pageViewModel;
                return ViewModel;
            }
        }


        public static ProductOptionViewModel GetProductById(int id)
        {
            ProductOptionViewModel productAtrributeViewModel = new ProductOptionViewModel();
            List<ProductAtrributeOptionsViewModel> viewModel = new List<ProductAtrributeOptionsViewModel>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                //ResponseViewModel<ProductAtrributeOptionsViewModel> response = new ResponseViewModel<ProductAtrributeOptionsViewModel>();
                List<Product_Attribute> options = new List<Product_Attribute>();
                List<ProductAttribute> attributes = new List<ProductAttribute>();
                //List<ProductAttributeOption> options = new List<ProductAttributeOption>();
                GetProductById_Result result = entities.GetProductById(id).FirstOrDefault();//get api of product
                Product product = new Product();
                product.Name = result.Name;
                product.ID = result.ID;
                product.Price = result.Price;
                product.ProductCateogoryID = result.CategoryId;
                product.ProductSubCategoryID = result.SubCategoryID;
                product.Image = result.Image;
                product.Description = result.Description;
                product.Discount = result.Discount;
                product.Quentity = result.Quentity;
                options = entities.Product_Attribute.Where(entry => entry.ProductID == id).ToList();
                //attributes = entities.ProductAttributes.Where(entry => entry. == product.ProductSubCategoryID).ToList();
                if (options != null)
                {
                    foreach (var option in options)
                    {
                        ProductAtrributeOptionsViewModel attributeOptionViewModel = new ProductAtrributeOptionsViewModel();
                        ProductAtrributeOptionsViewModel attributeOptionViewModelObj = new ProductAtrributeOptionsViewModel();
                        List<OptionList> optionsList = new List<OptionList>();
                        ProductAttributeOption optionDb = new ProductAttributeOption();
                        ProductAttribute productAttributeDb = new ProductAttribute();
                        optionDb = entities.ProductAttributeOptions.Where(entry => entry.ID == option.OptionID).FirstOrDefault();
                        productAttributeDb = entities.ProductAttributes.Where(entry => entry.ID == optionDb.ProductAtrributeID).FirstOrDefault();
                        attributeOptionViewModel.attributeName = productAttributeDb.Name;
                        attributeOptionViewModel.attributeId = Convert.ToInt32(productAttributeDb.ID);
                        if(viewModel.Any(entry => entry.attributeId == attributeOptionViewModel.attributeId))
                        {
                            attributeOptionViewModelObj = viewModel.Where(entry => entry.attributeId == attributeOptionViewModel.attributeId).FirstOrDefault();
                            if (attributeOptionViewModelObj != null)
                            {
                                OptionList optionViewModel = new OptionList();
                                optionViewModel.optionName = optionDb.Value;
                                optionViewModel.optionId = Convert.ToInt32(optionDb.ID);
                                attributeOptionViewModel.optionList = optionsList;
                                attributeOptionViewModelObj.optionList.Add(optionViewModel);
                            }   
                        }
                        else
                        {
                            OptionList optionViewModel = new OptionList();
                            optionViewModel.optionName = optionDb.Value;
                            optionViewModel.optionId = Convert.ToInt32(optionDb.ID);
                            optionsList.Add(optionViewModel);
                            attributeOptionViewModel.optionList = optionsList;
                            viewModel.Add(attributeOptionViewModel);
                        }
                    }
                }
                productAtrributeViewModel.product = product;
                productAtrributeViewModel.ViewModel = viewModel;
            }
            return productAtrributeViewModel;
        }


        public static ResponseViewModel<List<GetProductWiseOrders_Result>> GetProductWiseOrder()
        {
            ResponseViewModel<List<GetProductWiseOrders_Result>> response = new ResponseViewModel<List<GetProductWiseOrders_Result>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                /*List<Product> products = new List<Product>();
                products = entities.Products.ToList();
                foreach(var product in products)
                {
                    ProductWiseOrderViewModel viewModel = new ProductWiseOrderViewModel();
                    viewModel.ProductName = product.Name;
                    viewModel.NoOfOrders = 0;
                    viewModelList.Add(viewModel);
                }
                List<GetProductWiseOrders_Result> result = entities.GetProductWiseOrders().ToList();
                foreach (var product in viewModelList)
                {
                    foreach (var singleResult in result)
                    {
                        if (singleResult.ProductName == product.ProductName)
                        {
                            product.NoOfOrders = Convert.ToInt32(singleResult.NoOfOrders);
                        }
                        product.OrderCount = Convert.ToInt32(singleResult.OrderCount);
                        product.ProductCount = Convert.ToInt32(singleResult.ProductCount);
                        product.CustomerCount = Convert.ToInt32(singleResult.CustomerCount);
                    }
                }*/
                List<GetProductWiseOrders_Result> result = entities.GetProductWiseOrders().ToList();
                response.Data = result;
                return response;
            }
        }


        public static ResponseViewModel<List<GetProductByCategory_Result>> GetProductByCategory(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<List<GetProductByCategory_Result>> response = new ResponseViewModel<List<GetProductByCategory_Result>>();
                List<GetProductByCategory_Result> result = entities.GetProductByCategory(id).ToList();
                response.Data = result;
                return response;
            }
        }


        public static void DeleteProductQuentity(int productId,int orderedQuentity)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Product product = entities.Products.Where(entry => entry.ID == productId).FirstOrDefault();
                if (product != null)
                {
                    product.Quentity = product.Quentity - orderedQuentity;
                }
                entities.SaveChanges();
            }
        }


        public static Product UpdateProduct()
        {
            string imageName = null;
            HttpRequest httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["Image"];
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);
            Product productObj = new Product();
            int id = Convert.ToInt16(httpRequest["id"]);
            productObj = JsonConvert.DeserializeObject<Product>(httpRequest["product"]);
            productObj.Image = imageName;
            List<OptionViewModel> options = JsonConvert.DeserializeObject<List<OptionViewModel>>(httpRequest["option"]);
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Product productDb = entities.Products.Where(entry => entry.ID == id).FirstOrDefault();
                if (productDb != null)
                {
                    productDb.Name = productObj.Name;
                    productDb.Price = productObj.Price;
                    productDb.Image = productObj.Image;
                    productDb.ProductCateogoryID = productObj.ProductCateogoryID;
                    productDb.Discount = productObj.Discount;
                    productDb.Quentity = productObj.Quentity;
                    entities.SaveChanges();
                    foreach (var option in options)
                    {
                        Product_Attribute productAttribute = new Product_Attribute();
                        //option.productId = Convert.ToInt32(productObj.ID);
                        productAttribute.ProductID = productObj.ID;
                        productAttribute.OptionID = option.id;
                        if (ProductManager.CheckDuplicateOption(option.id, productObj.ID))
                        {
                            continue;
                        }
                        else
                        {
                            entities.Product_Attribute.Add(productAttribute);
                        }
                        entities.SaveChanges();
                    }
                }
            }
            return productObj;

        }



        public static int GetProductCount()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                int productCount = entities.Products.Count();
                return productCount;
            }
        }

        public static List<Product> GetProductsByOrder()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<Product> products = entities.Products.OrderBy(product => product.Name).ToList();
                return products;
            }
        }

       /* public static List<Product_BasketItem_Join> GetProductByBasket()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<Product_BasketItem_Join> products = entities.Products.Join(entities.BasketItems, product => product.ID, basketItem => basketItem.productId, (product, basketItem) => new Product_BasketItem_Join() { productName = product.Name, productPrice = product?.Price , productQuentity = basketItem.quantity }).Where(basketItem => basketItem.productQuentity == 3).ToList();
                return products; 
            }
        }*/

        public static List<IGrouping<string, Product>> GetProductsByGroup()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<IGrouping<string, Product>> products = entities.Products.GroupBy(product => product.Description).ToList();
                return products;
            }
        }

        public static ResponseViewModel<List<Product>> AddProduct()
        {
            ResponseViewModel<List<Product>> responseViewModel = new ResponseViewModel<List<Product>>();
            string imageName = null;
            HttpRequest httpRequest = HttpContext.Current.Request;
            var postedFile = httpRequest.Files["Image"];
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);
            Product productObj = new Product();
            /*productObj.Name = httpRequest["name"];
            productObj.Price = Convert.ToInt32(httpRequest["price"]);
            productObj.Description = httpRequest["description"];
            productObj.ProductCateogoryID = Convert.ToInt32(httpRequest["category"]);
            productObj.Quentity = Convert.ToInt32(httpRequest["quentity"]);
            productObj.Discount = Convert.ToInt32(httpRequest["discount"]);
            productObj.ProductSubCategoryID = Convert.ToInt32(httpRequest["subcategory"]);*/
            productObj = JsonConvert.DeserializeObject<Product>(httpRequest["product"]);
            productObj.Image = imageName;
            List<OptionViewModel> options = JsonConvert.DeserializeObject<List<OptionViewModel>>(httpRequest["option"]);
            using (EcommerceEntities entities = new EcommerceEntities())
            {

                if (CheckDuplicate(productObj))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 400;
                    return responseViewModel;
                }
                else
                {
                    entities.Products.Add(productObj);
                    responseViewModel.Data.Add(productObj);
                    entities.SaveChanges();
                    foreach(var option in options)
                    {
                        Product_Attribute productAttribute = new Product_Attribute();
                        //option.productId = Convert.ToInt32(productObj.ID);
                        productAttribute.ProductID = productObj.ID;
                        productAttribute.OptionID = option.id;
                        entities.Product_Attribute.Add(productAttribute);
                    }
                    entities.SaveChanges();
                }

            }
            return responseViewModel;
        }

        public static Boolean ProductOrdered(long productId)
        {
            using(EcommerceEntities entities = new EcommerceEntities())
            {
                if(entities.OrderItems.Any(entry => entry.ProductID == productId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }




        public static ResponseViewModel<Product> DeleteProduct(int id)
        {
            ResponseViewModel<Product> viewModel = new ResponseViewModel<Product>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Product product = entities.Products.Where(entry => entry.ID == id).FirstOrDefault();
                if (product != null)
                {
                    if (!ProductManager.ProductOrdered(product.ID))
                    {
                        entities.Products.Remove(product);
                        entities.SaveChanges();
                        viewModel.Data = product;
                    }
                    else
                    {
                        viewModel.errorViewModel = new ErrorViewModel();
                        viewModel.errorViewModel.statusCode = 300;
                    }
                }
                return viewModel;
            }
        }

        public static Boolean CheckDuplicateOption(int optionID,long productId)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.Product_Attribute.Any(entry => entry.OptionID == optionID && entry.ProductID == productId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public static Boolean CheckDuplicate(Product product)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if(entities.Products.Any(entry => entry.Name == product.Name && entry.ProductCateogoryID == product.ProductCateogoryID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
