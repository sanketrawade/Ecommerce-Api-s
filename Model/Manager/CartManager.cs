using Model.Data;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.Manager
{
    public class CartManager
    {
        public static Cart GetCart(int cartId=0)
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                Cart cart = new Cart();
                if (cartId != 0)
                {
                    cart = entities.Carts.Find(cartId);
                }
                return cart;
            }
        }



        public static List<CartAttributeViewModel> GetCartItems(int cartId)
        {
            List<CartAttributeViewModel> response = new List<CartAttributeViewModel>();
            List<CartItemOption> cartItemOptions = new List<CartItemOption>();
            Cart cart = GetCart(cartId);
            if (cart != null)
            {
                using (EcommerceEntities entities = new EcommerceEntities())
                {
                    List<GetCartItemsByCartId_Result> cartItems = entities.GetCartItemsByCartId(cartId).ToList();
                    foreach (var cartitem in cartItems)
                    {
                        ProductAtrributeOptionsViewModel productAtrributeViewModel = null;
                        List<ProductAtrributeOptionsViewModel> productAtrributeViewModelList = new List<ProductAtrributeOptionsViewModel>();
                        CartAttributeViewModel viewModel = new CartAttributeViewModel();
                        CartItemOption cartItemOption = new CartItemOption();
                        viewModel.cartItem = cartitem;
                        cartItemOptions = entities.CartItemOptions.Where(entry => entry.CartItemID == cartitem.ID).ToList();
                        foreach (var cartOption in cartItemOptions)
                        {
                            productAtrributeViewModel = new ProductAtrributeOptionsViewModel();
                            ProductAttributeOption optionDb = new ProductAttributeOption();
                            OptionList option = new OptionList();
                            List<OptionList> optionList = new List<OptionList>();
                            ProductAttribute productAttributeDb = new ProductAttribute();
                            optionDb = entities.ProductAttributeOptions.Where(entry => entry.ID == cartOption.OptionsID).FirstOrDefault();
                            productAttributeDb = entities.ProductAttributes.Where(entry => entry.ID == optionDb.ProductAtrributeID).FirstOrDefault();
                            option.optionId = Convert.ToInt32(cartOption.OptionsID);
                            option.optionName = optionDb.Value;
                            optionList.Add(option);
                            productAtrributeViewModel.attributeId = Convert.ToInt32(productAttributeDb.ID);
                            productAtrributeViewModel.attributeName = productAttributeDb.Name;
                            productAtrributeViewModel.optionList = optionList;
                            productAtrributeViewModelList.Add(productAtrributeViewModel);
                        }
                        viewModel.options = productAtrributeViewModelList;
                        response.Add(viewModel);
                    }
                }
            }
            return response;
        }

        public static List<Tax> GetTax()
        {
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                List<Tax> taxValues = entities.Taxes.ToList();
                return taxValues;
            }
        }

        public static ResponseViewModel<CartItem> AddItemtoCart(IdViewModel viewModel)
        {
            List<OptionList> optionList = new List<OptionList>();
            optionList = viewModel.optionsList;
            Cart cart = GetCart(viewModel.Id);
            ResponseViewModel<CartItem> responseViewModel = new ResponseViewModel<CartItem>();
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                CartItem cartItemDb = entities.CartItems.Where(entry => entry.ProductID == viewModel.ProductId && entry.CartID == cart.ID).FirstOrDefault();
                if (cartItemDb != null)
                {
                    Product productDb = entities.Products.Where(entry => entry.ID == viewModel.ProductId).FirstOrDefault();
                    if (cartItemDb.Quentity != productDb.Quentity)
                    {
                        cartItemDb.Quentity = cartItemDb.Quentity + 1;
                        responseViewModel.Data = cartItemDb;
                    }
                    else
                    {
                        responseViewModel.errorViewModel = new ErrorViewModel();
                        responseViewModel.errorViewModel.statusCode = 400;
                    }
                }
                else
                {
                    CartItem cartItem = new CartItem();
                    cartItem.ProductID = viewModel.ProductId;
                    cartItem.Quentity = 1;
                    cartItem.CartID = Convert.ToInt16(cart.ID);
                    entities.CartItems.Add(cartItem);
                    responseViewModel.Data = cartItem;
                    entities.SaveChanges();
                    foreach (var option in optionList)
                    {
                        CartItemOption cartItemOption = new CartItemOption();
                        cartItemOption.CartItemID = cartItem.ID;
                        cartItemOption.OptionsID = option.optionId;
                        entities.CartItemOptions.Add(cartItemOption);
                    }
                    entities.SaveChanges();
                }
                entities.SaveChanges();
            }
            return responseViewModel;
        }

        public static void RemoveSingleItemFromCart(int cartId,int productId)
        {
            Cart cart = GetCart(cartId);
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                CartItem cartItemDb = entities.CartItems.Where(entry => entry.ProductID == productId && entry.CartID == cart.ID).FirstOrDefault();
                if (cartItemDb.Quentity > 1)
                {
                    cartItemDb.Quentity = cartItemDb.Quentity - 1;
                    entities.SaveChanges();
                }
            }
        }

        public static void RemoveCartItem(int cartId,int cartItemId)
        {
            Cart cart = GetCart(cartId);
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                CartItem cartItemDb = entities.CartItems.Where(entry => entry.ID == cartItemId && entry.CartID == cart.ID).FirstOrDefault();
                if (cartItemDb != null)
                {
                    entities.CartItems.Remove(cartItemDb);
                    entities.SaveChanges();
                }
            }
        }
        
        public static void RemoveAllBCartItems(int cartId)
        {
            Cart cart = GetCart(cartId);
            if (cart != null)
            {
                using (EcommerceEntities entities = new EcommerceEntities())
                {
                    List<CartItem> cartItems = entities.CartItems.Where(entry => entry.CartID == cart.ID).ToList();
                    if(cartItems.Count != 0)
                    {
                        foreach(CartItem item in cartItems)
                        {
                            entities.CartItems.Remove(item);
                        }
                        entities.SaveChanges();
                    }
                }
            }
        }

        public static int GetCartItemCount(int cartId)
        {
            int count = 0;
            Cart cart = GetCart(cartId);
            using (EcommerceEntities entities = new EcommerceEntities())
            {
                count = entities.CartItems.Where(entry => entry.CartID == cart.ID).Count();
                return count;
            }
        }

    }
}