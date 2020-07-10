using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ResponseViewModel<T>
    {
        public T Data;
        public ErrorViewModel errorViewModel { get; set; }
    }
}
