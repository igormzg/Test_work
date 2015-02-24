using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products_grid.Models.Products
{
    public class Product
    {
        public int id { get; set; }

        public string name { get; set; }

        public float price { get; set; }

        public int count { get; set; }
    }
}