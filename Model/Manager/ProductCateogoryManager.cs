using Model.Data;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manager
{
    #region category
    public class ProductCateogoryManager
    {
        public static ResponseViewModel<List<ProductCateogory>> GetProductCategories()
        {
            ResponseViewModel<List<ProductCateogory>> viewModel = new ResponseViewModel<List<ProductCateogory>>();
            using(EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductCateogory> productCategory = entities.ProductCateogories.ToList();
                viewModel.Data = productCategory;
            }
            return viewModel;
        }

        public static ResponseViewModel<PageViewModel<GetCategoryList_Result>> GetProductCategoryList(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<GetCategoryList_Result>> ViewModel = new ResponseViewModel<PageViewModel<GetCategoryList_Result>>();
                List<GetCategoryList_Result> getProductResult = new List<GetCategoryList_Result>();
                PageViewModel<GetCategoryList_Result> pageViewModel = new PageViewModel<GetCategoryList_Result>();
                getProductResult = entities.GetCategoryList(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == "" ? null : searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
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

        public static ResponseViewModel<ProductCateogory> GetProductCategoryById(int id)
        {
            ResponseViewModel<ProductCateogory> viewModel = new ResponseViewModel<ProductCateogory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductCateogory productCategory = entities.ProductCateogories.Where(entry => entry.ID == id).FirstOrDefault();
                viewModel.Data = productCategory;
            }
            return viewModel;
        }

        public static ResponseViewModel<ProductCateogory> AddProductCategory(ProductCateogory productCategory)
        {
            ResponseViewModel<ProductCateogory> responseViewModel = new ResponseViewModel<ProductCateogory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (CheckDuplicate(productCategory))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 400;
                }
                else
                {
                    entities.ProductCateogories.Add(productCategory);
                    responseViewModel.Data = productCategory;
                    entities.SaveChanges();
                }
            }
            return responseViewModel;
        }

        public static ResponseViewModel<ProductCateogory> UpdateProductCategory(ProductCateogory productCategory)
        {
            ResponseViewModel<ProductCateogory> responseViewModel = new ResponseViewModel<ProductCateogory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductCateogory productCategoryDb = entities.ProductCateogories.Where(entry => entry.ID == productCategory.ID).FirstOrDefault();
                productCategoryDb.Name = productCategory.Name;
                responseViewModel.Data = productCategoryDb;
                entities.SaveChanges();
            }
            return responseViewModel;
        }

        public static void DeleteProductCategory(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductCateogory productCategory = entities.ProductCateogories.Where(entry => entry.ID == id).FirstOrDefault();
                entities.ProductCateogories.Remove(productCategory);
                entities.SaveChanges();
            }
        }

        public static Boolean CheckDuplicate(ProductCateogory productCategory)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.ProductCateogories.Any(entry => entry.Name == productCategory.Name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    #endregion
    #region subcategory
    public class ProductSubCategoryManager
    {
        public static ResponseViewModel<List<ProductSubCategory>> GetProducSubtCategories()
        {
            ResponseViewModel<List<ProductSubCategory>> viewModel = new ResponseViewModel<List<ProductSubCategory>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductSubCategory> productSubCategory = entities.ProductSubCategories.ToList();
                viewModel.Data = productSubCategory;
            }
            return viewModel;
        }

        public static ResponseViewModel<PageViewModel<GetSubCategoryList_Result>> GetProductCategoryList(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<GetSubCategoryList_Result>> ViewModel = new ResponseViewModel<PageViewModel<GetSubCategoryList_Result>>();
                List<GetSubCategoryList_Result> getProductResult = new List<GetSubCategoryList_Result>();
                PageViewModel<GetSubCategoryList_Result> pageViewModel = new PageViewModel<GetSubCategoryList_Result>();
                getProductResult = entities.GetSubCategoryList(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == "" ? null : searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
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

        public static ResponseViewModel<ProductSubCategory> GetProductSubCategoryById(int id)
        {
            ResponseViewModel<ProductSubCategory> viewModel = new ResponseViewModel<ProductSubCategory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductSubCategory productCategory = entities.ProductSubCategories.Where(entry => entry.ID == id).FirstOrDefault();
                viewModel.Data = productCategory;
            }
            return viewModel;
        }

        public static ResponseViewModel<List<ProductSubCategory>> GetSubcategoryByCategory(int id)
        {
            ResponseViewModel<List<ProductSubCategory>> viewModel = new ResponseViewModel<List<ProductSubCategory>>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<ProductSubCategory> productCategory = entities.ProductSubCategories.Where(entry => entry.ProductCategoryID == id).ToList();
                viewModel.Data = productCategory;
            }
            return viewModel;
        }

        public static ResponseViewModel<ProductSubCategory> AddProductSubCategory(ProductSubCategory productSubCategory)
        {
            ResponseViewModel<ProductSubCategory> responseViewModel = new ResponseViewModel<ProductSubCategory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (ProductSubCategoryManager.CheckDuplicate(productSubCategory))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 400;
                }
                else
                {
                    entities.ProductSubCategories.Add(productSubCategory);
                    responseViewModel.Data = productSubCategory;
                    entities.SaveChanges();
                }
            }
            return responseViewModel;
        }

        public static ResponseViewModel<ProductSubCategory> UpdateProductSubCategory(ProductSubCategory productSubCategory)
        {
            ResponseViewModel<ProductSubCategory> responseViewModel = new ResponseViewModel<ProductSubCategory>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (ProductSubCategoryManager.CheckDuplicate(productSubCategory))
                {
                    responseViewModel.errorViewModel = new ErrorViewModel();
                    responseViewModel.errorViewModel.statusCode = 400;
                }
                else
                {
                    ProductSubCategory productSubCategoryDb = entities.ProductSubCategories.Where(entry => entry.ID == productSubCategory.ID).FirstOrDefault();
                    productSubCategoryDb.Name = productSubCategory.Name;
                    productSubCategoryDb.ProductCategoryID = productSubCategory.ProductCategoryID;
                    responseViewModel.Data = productSubCategoryDb;
                    entities.SaveChanges();
                }
            }
            return responseViewModel;
        }

        public static void DeleteProductSubCategory(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ProductSubCategory productSubCategory = entities.ProductSubCategories.Where(entry => entry.ID == id).FirstOrDefault();
                entities.ProductSubCategories.Remove(productSubCategory);
                entities.SaveChanges();
            }
        }

        public static Boolean CheckDuplicate(ProductSubCategory productSubCategory)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                if (entities.ProductSubCategories.Any(entry => entry.Name == productSubCategory.Name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    #endregion

}
