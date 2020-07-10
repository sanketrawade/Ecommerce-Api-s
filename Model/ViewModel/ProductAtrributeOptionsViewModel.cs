using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductAtrributeOptionsViewModel
    {
        public int attributeId { get; set; }
        public string attributeName { get; set; }
        public virtual List<OptionList> optionList { get; set; }
    }
}
