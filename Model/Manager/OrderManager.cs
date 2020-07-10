using Model.Data;
using Model.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Model.Manager
{
    public class OrderManager
    {
        #region Order
        public static void GenerateOrder()
        {
            using (EcommerceEntities entites = new EcommerceEntities())
            {
                HttpRequest httpRequest = HttpContext.Current.Request;
                List<OrderItemViewModel> orderItems = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(httpRequest["orderItems"]);
                Order order = new Order();
                order.CustomerID = Convert.ToInt16(httpRequest["custmerId"]);
                order.GrandTotal = Convert.ToInt16(httpRequest["GrandTotal"]);
                order.Date = DateTime.UtcNow;
                OrderStatu orderStatus = entites.OrderStatus.FirstOrDefault();
                order.OrderStatusID = orderStatus.ID;
                foreach (var orderItem in orderItems)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ItemName = orderItem.itemName,
                        ItemDiscount = orderItem.itemDiscount,
                        ItemImage = orderItem.itemImage,
                        ItemPrice = Convert.ToInt16(orderItem.itemPrice),
                        ItemQuentity = Convert.ToInt16(orderItem.itemQuentity),
                        ProductID = orderItem.productId
                    });
                }
                entites.Orders.Add(order);
                entites.SaveChanges();   
            }
        }

        public static ResponseViewModel<PageViewModel<OrderDetailsViewModel>> GetOrderDetails(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<OrderDetailsViewModel>> ViewModel = new ResponseViewModel<PageViewModel<OrderDetailsViewModel>>();
                List<GetOrders_Result> getOrderResult = new List<GetOrders_Result>();
                PageViewModel<OrderDetailsViewModel> pageViewModel = new PageViewModel<OrderDetailsViewModel>();
                getOrderResult = entities.GetOrders(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == "" ? null : searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
                List<OrderDetailsViewModel> orderDetails = new List<OrderDetailsViewModel>();
               //pageViewModel.Data = getOrderResult;
                int i = 1;
                Boolean flag = false;
                foreach(var singleOrderDetail in getOrderResult)
                {
                    OrderDetailsViewModel orderDetail = new OrderDetailsViewModel();
                    flag = false;
                    if (i == 1)
                    {
                        CustomerViewModel customerViewModel = new CustomerViewModel()
                        {
                            firstName = singleOrderDetail.FirstName,
                            lastName = singleOrderDetail.LastName,
                            emailId = singleOrderDetail.EmailId,
                            city = singleOrderDetail.City,
                            street = singleOrderDetail.Street,
                            state = singleOrderDetail.State,
                            zipCode = singleOrderDetail.ZipCode
                        };
                        orderDetail.customerDetails = customerViewModel;
                        orderDetail.GrandTotal = Convert.ToInt32(singleOrderDetail.GrandTotal);
                        orderDetail.orderStatus = singleOrderDetail.OrderStatusName;
                        orderDetail.orderId = Convert.ToInt32(singleOrderDetail.ID);
                        orderDetail.Date = singleOrderDetail.Date;
                        OrderItemViewModel orderItem = new OrderItemViewModel();
                        orderItem.itemName = singleOrderDetail.ItemName;
                        orderItem.itemDiscount = Convert.ToInt32(singleOrderDetail.ItemDiscount);
                        orderItem.itemImage = singleOrderDetail.ItemImage;
                        orderItem.itemPrice = singleOrderDetail.ItemPrice.ToString();
                        orderItem.itemQuentity = singleOrderDetail.ItemQuetity.ToString();
                        orderItem.productId = 1;
                        orderDetail.orderItemDetails = new List<OrderItemViewModel>();
                        orderDetail.orderItemDetails.Add(orderItem);
                        orderDetails.Add(orderDetail);
                    }
                    else
                    {
                        foreach(var data in orderDetails)
                        {
                            if(singleOrderDetail.ID == data.orderId)
                            {
                                OrderItemViewModel orderItem = new OrderItemViewModel();
                                orderItem.itemName = singleOrderDetail.ItemName;
                                orderItem.itemDiscount = Convert.ToInt32(singleOrderDetail.ItemDiscount);
                                orderItem.itemImage = singleOrderDetail.ItemImage;
                                orderItem.itemPrice = singleOrderDetail.ItemPrice.ToString();
                                orderItem.itemQuentity = singleOrderDetail.ItemQuetity.ToString();
                                data.orderItemDetails.Add(orderItem);
                                flag = true;
                            }
                        }
                        if(flag != true)
                        {
                            CustomerViewModel customerViewModel = new CustomerViewModel()
                            {
                                firstName = singleOrderDetail.FirstName,
                                lastName = singleOrderDetail.LastName,
                                emailId = singleOrderDetail.EmailId,
                                city = singleOrderDetail.City,
                                street = singleOrderDetail.Street,
                                state = singleOrderDetail.State,
                                zipCode = singleOrderDetail.ZipCode
                            };
                            orderDetail.customerDetails = customerViewModel;
                            orderDetail.GrandTotal = Convert.ToInt32(singleOrderDetail.GrandTotal);
                            orderDetail.orderStatus = singleOrderDetail.OrderStatusName;
                            orderDetail.orderId = Convert.ToInt32(singleOrderDetail.ID);
                            orderDetail.Date = singleOrderDetail.Date;
                            OrderItemViewModel orderItem = new OrderItemViewModel();
                            orderItem.itemName = singleOrderDetail.ItemName;
                            orderItem.itemDiscount = Convert.ToInt32(singleOrderDetail.ItemDiscount);
                            orderItem.itemImage = singleOrderDetail.ItemImage;
                            orderItem.itemPrice = singleOrderDetail.ItemPrice.ToString();
                            orderItem.itemQuentity = singleOrderDetail.ItemQuetity.ToString();
                            orderDetail.orderItemDetails = new List<OrderItemViewModel>();
                            orderDetail.orderItemDetails.Add(orderItem);
                            orderDetails.Add(orderDetail);
                        }
                    }
                    i = i + 1;
                }
                pageViewModel.Data = orderDetails;
                pageViewModel.totalRecords = getOrderResult.Count();
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

        public static ResponseViewModel<List<GetOrderByCustomerId_Result>> GetOrderDetailsByCustomerID(int customerId)
        {
            ResponseViewModel<List<GetOrderByCustomerId_Result>> viewModel = new ResponseViewModel<List<GetOrderByCustomerId_Result>>();
            using (EcommerceEntities entites = new EcommerceEntities())
            {
                viewModel.Data = entites.GetOrderByCustomerId(customerId).ToList();
                if (viewModel.Data != null)
                {
                    return viewModel;
                }
            }
            viewModel.errorViewModel = new ErrorViewModel();
            viewModel.errorViewModel.statusCode = 500;
            return viewModel;
        }

        public static void DeleteOrderDetails(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Order order = entities.Orders.Where(entry => entry.ID == id).FirstOrDefault();
                List<OrderItem> orderItems = entities.OrderItems.Where(entry => entry.OrderID == id).ToList();
                foreach(var orderItem in orderItems)
                {
                    entities.OrderItems.Remove(orderItem);
                }
                entities.Orders.Remove(order);
                entities.SaveChanges();
            }
        }


        #endregion
        #region OrderStatus
        public static ResponseViewModel<PageViewModel<GetOrderStatusList_Result>> GetOrderStatus(SearchViewModel searchViewModel)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                ResponseViewModel<PageViewModel<GetOrderStatusList_Result>> ViewModel = new ResponseViewModel<PageViewModel<GetOrderStatusList_Result>>();
                List<GetOrderStatusList_Result> getProductResult = new List<GetOrderStatusList_Result>();
                PageViewModel<GetOrderStatusList_Result> pageViewModel = new PageViewModel<GetOrderStatusList_Result>();
                getProductResult = entities.GetOrderStatusList(searchViewModel.pageIndex, searchViewModel.pageSize, searchViewModel.searchText == "" ? null : searchViewModel.searchText, searchViewModel.sortOrder, searchViewModel.sortColoumn).ToList();
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

        public static ResponseViewModel<OrderStatu> GetOrderStatus(int id)
        {
            ResponseViewModel<OrderStatu> viewModel = new ResponseViewModel<OrderStatu>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                OrderStatu orderStatus = entities.OrderStatus.Where(entry => entry.ID == id).FirstOrDefault();
                viewModel.Data = orderStatus;
            }
            return viewModel;
        }

        public static ResponseViewModel<Order> GetOrderStatusByOrderId(int id)
        {
            ResponseViewModel<Order> viewModel = new ResponseViewModel<Order>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Order order = entities.Orders.Where(entry => entry.ID == id).FirstOrDefault();
                viewModel.Data = order;
            }
            return viewModel;
        }


        public static ResponseViewModel<OrderStatu> AddOrderStatus(OrderStatu orderStatus)
        {
            ResponseViewModel<OrderStatu> viewModel = new ResponseViewModel<OrderStatu>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                entities.OrderStatus.Add(orderStatus);
                entities.SaveChanges();
                viewModel.Data = orderStatus;
            }
            return viewModel;
        }

        public static ResponseViewModel<OrderStatu> UpdateOrderStatus(OrderStatu ordersStatus)
        {
            ResponseViewModel<OrderStatu> viewModel = new ResponseViewModel<OrderStatu>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                OrderStatu orderStatusDb = entities.OrderStatus.Where(entry => entry.ID == ordersStatus.ID).FirstOrDefault();
                orderStatusDb.Name = ordersStatus.Name;
                viewModel.Data = orderStatusDb;
                entities.SaveChanges();
            }
            return viewModel;
        }

        public static ResponseViewModel<Order> UpdateOrderStatusOfOrder(int orderId,int statusId)
        {
            ResponseViewModel<Order> viewModel = new ResponseViewModel<Order>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Order orderDb = entities.Orders.Where(entry => entry.ID == orderId).FirstOrDefault();
                if (orderDb != null)
                {
                    orderDb.OrderStatusID = statusId;
                    viewModel.Data = orderDb;
                    entities.SaveChanges();
                }
            }
            return viewModel;
        }

        public static void DeleteOrderStatus(int id)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                OrderStatu orderStatus = entities.OrderStatus.Where(entry => entry.ID == id).FirstOrDefault();
                entities.OrderStatus.Remove(orderStatus);
                entities.SaveChanges();
            }
        }
        #endregion
    }
}
