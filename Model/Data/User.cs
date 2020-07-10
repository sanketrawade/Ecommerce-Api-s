//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Customers = new HashSet<Customer>();
        }
    
        public long ID { get; set; }
        public Nullable<long> RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}