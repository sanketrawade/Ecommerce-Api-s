using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CartAttributeViewModel
    {
        public GetCartItemsByCartId_Result cartItem { get; set; }
        public List<ProductAtrributeOptionsViewModel> options { get; set; }
    }
}
