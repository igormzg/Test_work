//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Products_grid.Models.Products
{
    using System;
    using System.Collections.Generic;
    
    public partial class Operations
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public int Count { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual OperationType OperationType { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}