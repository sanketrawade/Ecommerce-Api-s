using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PasswordViewModel
    {
        public int id { get; set; }
        public string emailId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
