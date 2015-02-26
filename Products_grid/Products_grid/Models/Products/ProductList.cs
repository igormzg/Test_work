using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Products_grid.Models.Products
{
    public class ProductList
    {
        ProductsContext _context;

        public ProductList (ProductsContext context)
        {
            _context = context;
        }

        public List<Product> GetProductRange(int firstIndex, int rows)
        {
            List<Product> products = _context.Products.ToList();

            if ((products.Count() - firstIndex) >= rows)
            {
                products = products.GetRange(firstIndex, rows);
            }
            else
            {
                products = products.GetRange(firstIndex, products.Count() - firstIndex);
            }

            return products;
        }

        public List<Product> GetProductRange(int rows)
        {
            return GetProductRange(0, rows);
        }
    }
}