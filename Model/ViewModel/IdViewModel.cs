using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class IdViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int status { get; set; }
        public string emailId { get; set; }
        public string password { get; set; }
        public Boolean  registrationFlag { get; set; }
        public List<OptionList> optionsList { get; set; }
    }
}
