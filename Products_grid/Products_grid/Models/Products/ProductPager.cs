using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Products_grid.Models.Products
{
    public class ProductPager
    {
        ProductsContext _context;

        public ProductPager(ProductsContext context)
        {
            _context = context;
        }

        public List<Product> GetPage(int firstIndex, int rows)
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

        public List<Product> GetPage(int rows)
        {
            return GetPage(0, rows);
        }
    }
}