using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PageViewModel<T>
    {
        public  List<T> Data { get; set; }
        public int totalRecords { get; set; }
        public int pageIndex { get; set; }
        public int totalPage { get; set; }
    }
}