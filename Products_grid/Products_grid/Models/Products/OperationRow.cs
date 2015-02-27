using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products_grid.Models.Products
{
    public class OperationRow
    {
        public int Id {get; set;}
         
        public string User {get; set;}

        public string Product {get; set;}

        public bool Type {get; set;}

        public int Count {get; set;}

        public DateTime Date {get; set;}
    }
}