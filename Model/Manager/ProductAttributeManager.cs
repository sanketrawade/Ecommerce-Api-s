using Model.Data;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manager
{
    public class ProductAttributeManager
    {
        #region Atrribute
        public static ResponseViewModel<List<ProductAttribute>> GetProductAttribute()
        {
            ResponseViewModel<List<ProductAttribute>> viewModel = new ResponseViewModel<List<ProductAttribute>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttribute> productAtrribbute = entities.ProductAttributes.ToList();
                viewModel.Data = productAtrribbute;
            }
            return viewModel;
        }

        public static ResponseViewModel<PageViewModel<GetAttributeList_Result>> GetProductAttributeList(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<GetAttributeList_Result>> ViewModel = new ResponseViewModel<PageViewModel<GetAttributeList_Result>>();
                List<GetAttributeList_Result> getProductResult = new List<GetAttributeList_Result>();
                PageViewModel<GetAttributeList_Result> pageViewModel = new PageViewModel<GetAttributeList_Result>();
                getProductResult = entities.GetAttributeList(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == "" ? null : searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
                pageViewModel.Data = getProductResult;
                pageViewModel.totalRecords = getProductResult.Count();
                if (((pageViewModel.totalRecords % searchViewModel.pageSize) == 0))
                {
                    if (pageViewModel.totalRecords < searchViewModel.pageSize)
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

        public static ProductAttributeViewModel GetProductAtrributeById(int id)
        {
            ResponseViewModel<List<ProductAttributeViewModel>> viewModel = new ResponseViewModel<List<ProductAttributeViewModel>>();
            ProductAttributeViewModel attributeViewModel = new ProductAttributeViewModel();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductAttribute productAtrribute = new ProductAttribute();
                productAtrribute = entities.ProductAttributes.Where(entry => entry.ID == id).FirstOrDefault();
                //List<ProductAttributeOption> options = new List<ProductAttributeOption>();
                //options = entities.ProductAttributeOptions.Where(entry => entry.ProductAtrributeID == productAtrribute.ID).ToList();
                //attributeViewModel.productAttribute = productAtrribute;
                //attributeViewModel.options = options;
                attributeViewModel.attributeName = productAtrribute.Name;
                attributeViewModel.productCategoryId = Convert.ToInt32(productAtrribute.ProductCategoryID);
                attributeViewModel.productSubCategoryId = Convert.ToInt32(productAtrribute.ProductSubCategoryID);
            }
            return attributeViewModel;
        }

        public static List<ProductAtrributeOptionsViewModel> GetProductAtrributeOptionById(int id)
        {
            ResponseViewModel<ProductAtrributeOptionsViewModel> viewModel = new ResponseViewModel<ProductAtrributeOptionsViewModel>();
            List<ProductAtrributeOptionsViewModel> attributeOptionViewModelList = new List<ProductAtrributeOptionsViewModel>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttributeOption> options = new List<ProductAttributeOption>();
                List<ProductAttribute> attributes = new List<ProductAttribute>();
                List<OptionList> optionsList = null;
                attributes = entities.ProductAttributes.Where(entry => entry.ProductSubCategoryID == id).ToList();
                foreach (var attribute in attributes)
                {
                    ProductAtrributeOptionsViewModel attributeOptionViewModel = new ProductAtrributeOptionsViewModel();
                    attributeOptionViewModel.attributeName = attribute.Name;
                    attributeOptionViewModel.attributeId = Convert.ToInt32(attribute.ID);
                    options = entities.ProductAttributeOptions.Where(entry => entry.ProductAtrributeID == attribute.ID).ToList();
                    if (options != null)
                    {
                        optionsList = new List<OptionList>();
                        foreach (var option in options)
                        {
                            OptionList optionViewModel = new OptionList();
                            optionViewModel.optionName = option.Value;
                            optionViewModel.optionId = Convert.ToInt32(option.ID);
                            optionsList.Add(optionViewModel);
                        }
                    }
                    attributeOptionViewModel.optionList = optionsList;
                    attributeOptionViewModelList.Add(attributeOptionViewModel);
                }
                return attributeOptionViewModelList;
            }
        }


        public static ResponseViewModel<List<ProductAttribute>> GetProductAtrributeBySubCategory(int id)
        {
            ResponseViewModel<List<ProductAttribute>> viewModel = new ResponseViewModel<List<ProductAttribute>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttribute> productAttribute = entities.ProductAttributes.Where(entry => entry.ProductSubCategoryID == id).ToList();
                viewModel.Data = productAttribute;
            }
            return viewModel;
        }

        public static ResponseViewModel<ProductAttribute> AddProductAttribute(ProductAttributeViewModel viewModel)
        {
            ResponseViewModel<ProductAttribute> responseViewModel = new ResponseViewModel<ProductAttribute>();
            ProductAttribute productAtribute = new ProductAttribute();
            productAtribute.Name = viewModel.attributeName;
            productAtribute.ProductCategoryID = viewModel.productCategoryId;
            productAtribute.ProductSubCategoryID = viewModel.productSubCategoryId;
            using (EcommerceEntities entities = new EcommerceEntities())
            { 
                if (CheckDuplicate(productAtribute))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 400;
                }
                else
                {
                    entities.ProductAttributes.Add(productAtribute);
                    entities.SaveChanges();
                    ProductAttributeManager.AddOption(viewModel.options,productAtribute.ID);
                    responseViewModel.Data = productAtribute;
                    entities.SaveChanges();
                }
            }
            return responseViewModel;
        }

        public static ResponseViewModel<ProductAttribute> UpdateProductAttribute(ProductAttributeViewModel viewModel)
        {
            ResponseViewModel<ProductAttribute> responseViewModel = new ResponseViewModel<ProductAttribute>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductAttribute productAtributeDb = entities.ProductAttributes.Where(entry => entry.ID == viewModel.ID).FirstOrDefault();
                productAtributeDb.Name = viewModel.attributeName;
                productAtributeDb.ProductCategoryID = viewModel.productCategoryId;
                productAtributeDb.ProductSubCategoryID = viewModel.productSubCategoryId;
                entities.SaveChanges();
                ProductAttributeManager.AddOption(viewModel.options, productAtributeDb.ID);
                responseViewModel.Data = productAtributeDb;
                entities.SaveChanges();
                return responseViewModel;
            }
        }

        public static void DeleteProductAttribute(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductAttribute productAttribute = entities.ProductAttributes.Where(entry => entry.ID == id).FirstOrDefault();
                ProductAttributeManager.DeleteOptionByAttributeId(productAttribute.ID);
                entities.ProductAttributes.Remove(productAttribute);
                entities.SaveChanges();
            }
        }

        public static Boolean CheckDuplicate(ProductAttribute ProductAttribute)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.ProductAttributes.Any(entry => entry.Name == ProductAttribute.Name && entry.ProductCategoryID == ProductAttribute.ProductCategoryID && entry.ProductSubCategoryID == ProductAttribute.ProductSubCategoryID))
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
        #region AtrributeValues
        public static ResponseViewModel<List<ProductAttributeOption>> GetOptions()
        {
            ResponseViewModel<List<ProductAttributeOption>> viewModel = new ResponseViewModel<List<ProductAttributeOption>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttributeOption> propductAtrribute = entities.ProductAttributeOptions.ToList();
                viewModel.Data = propductAtrribute;
            }
            return viewModel;
        }

        public static ResponseViewModel<List<ProductAttributeOption>> GetOptionByID(int id)
        {
            ResponseViewModel<List<ProductAttributeOption>> viewModel = new ResponseViewModel<List<ProductAttributeOption>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttributeOption> productAttributeOption = entities.ProductAttributeOptions.Where(entry => entry.ProductAtrributeID == id).ToList();
                viewModel.Data = productAttributeOption;
            }
            return viewModel;
        }

        public static ResponseViewModel<List<ProductAttributeOption>> AddOption(List<ProductAttributeOption> values,long attributeId)
        {
            ResponseViewModel<List<ProductAttributeOption>> responseViewModel = new ResponseViewModel<List<ProductAttributeOption>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                foreach(var value in values)
                {
                    if (ProductAttributeManager.CheackDuplicateOption(value.Value,attributeId))
                    {
                        continue;
                    }
                    else
                    {
                        value.ProductAtrributeID = attributeId;
                        entities.ProductAttributeOptions.Add(value);
                    }
                }
                entities.SaveChanges();
                responseViewModel.Data = values;
            }
            return responseViewModel;
        }

        public static Boolean CheackDuplicateOption(string value,long attributeId)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if(entities.ProductAttributeOptions.Any(entry => entry.Value == value && entry.ProductAtrributeID == attributeId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static ResponseViewModel<ProductAttributeOption> UpdateOption(ProductAttributeOption productAttributeOption)
        {
            ResponseViewModel<ProductAttributeOption> responseViewModel = new ResponseViewModel<ProductAttributeOption>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductAttributeOption productAttributeOptionDb = entities.ProductAttributeOptions.Where(entry => entry.ID == productAttributeOption.ID).FirstOrDefault();
                productAttributeOptionDb.Value = productAttributeOption.Value;
                responseViewModel.Data = productAttributeOptionDb;
                entities.SaveChanges();
            }
            return responseViewModel;
        }


        public static void DeleteProductOptionById(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductAttributeOption propductAttributeOption = entities.ProductAttributeOptions.Where(entry => entry.ID == id).FirstOrDefault();
                entities.ProductAttributeOptions.Remove(propductAttributeOption);
                entities.SaveChanges();
            }
        }

        public static void DeleteOptionByAttributeId(long id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductAttributeOption> productAttributeOptions = entities.ProductAttributeOptions.Where(entry => entry.ProductAtrributeID == id).ToList();
                foreach(var productAttribute in productAttributeOptions)
                {
                    entities.ProductAttributeOptions.Remove(productAttribute);
                }
                entities.SaveChanges();
            }
        }
        #endregion
    }
}
