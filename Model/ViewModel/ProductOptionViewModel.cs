using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductOptionViewModel
    {
        public Product product { get; set; }
        public List<ProductAtrributeOptionsViewModel> ViewModel { get; set; }
    }
}
