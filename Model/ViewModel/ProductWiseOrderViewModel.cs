using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductWiseOrderViewModel
    {
        public string ProductName { get; set; }
        public int NoOfOrders { get; set; }
        public int OrderCount { get; set; }
        public int ProductCount { get; set; }
        public int CustomerCount { get; set; }
    }
}
