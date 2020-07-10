using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class SearchViewModel
    {
        public int pageIndex { get; set; }
        public string searchText { get; set; }
        public int pageSize { get; set; }
        public string sortColoumn { get; set; }
        public string sortOrder { get; set; }
    }
}
