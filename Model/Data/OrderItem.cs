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
    
    public partial class OrderItem
    {
        public long ID { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> ItemPrice { get; set; }
        public Nullable<int> ItemQuentity { get; set; }
        public string ItemImage { get; set; }
        public Nullable<int> ItemDiscount { get; set; }
        public Nullable<long> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
    
        public virtual Order Order { get; set; }
    }
}