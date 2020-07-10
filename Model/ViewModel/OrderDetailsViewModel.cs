using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class OrderDetailsViewModel
    {
        public CustomerViewModel customerDetails { get; set; }
        public List<OrderItemViewModel> orderItemDetails { get; set; }
        public int orderId { get; set; }
        public int GrandTotal { get; set; }
        public string Date  { get; set; }
        public string orderStatus { get; set; }
    }
}
