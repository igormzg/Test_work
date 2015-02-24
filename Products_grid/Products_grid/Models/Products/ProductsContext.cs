using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Products_grid.Models.Products
{
    public class ProductsContext : DbContext
    {
        public ProductsContext() : base("DefaultConnection") { }

        public DbSet<Product> Products { get; set; }
    }
}