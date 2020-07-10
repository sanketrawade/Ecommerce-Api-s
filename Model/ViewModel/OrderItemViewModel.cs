using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class OrderItemViewModel
    {
        public string itemName { get; set; }
        public string itemPrice { get; set; }
        public string itemQuentity { get; set; }
        public string itemImage { get; set; }
        public int itemDiscount { get; set; }
        public int productId { get; set; }
    }
}
