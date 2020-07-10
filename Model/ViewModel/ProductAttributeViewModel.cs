using Model.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductAttributeViewModel
    {
        public int ID { get; set; }
        public List<ProductAttributeOption> options { get; set; }
        public ProductAttribute productAttribute { get; set; }
        public string attributeName { get; set; }
        public int productCategoryId { get; set; }
        public int productSubCategoryId { get; set; }
    }
}
