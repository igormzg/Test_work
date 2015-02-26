using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products_grid.Models.Products;
using Newtonsoft.Json;

namespace Products_grid.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.TypeList = ;

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                _prodContext.Products.Add(product);
                _prodContext.SaveChanges();
                return View("Index");
            }
        }

        public string GetProducts()
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                ProductList list = new ProductList(_prodContext);
                int rowCount = 10;
                int firstIndex = (Convert.ToInt32(Request.Params["page"]) - 1) * rowCount;
                List<Product> products = list.GetProductRange(firstIndex, rowCount);
                List<Object> anonValueList = new List<Object>();

                foreach (var j in products)
                {
                    anonValueList.Add(new
                    {
                        Id = j.Id,
                        Name = j.Name,
                        Price = j.Price,
                        Count = j.Count
                    });
                }

                string jsonResult = JsonConvert.SerializeObject(anonValueList.ToArray());
                return jsonResult;
            }
        }

        [HttpPost]
        public string DeleteProduct()
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                int id = Convert.ToInt32(Request.Params["Id"]);
                Product product = _prodContext.Products.Where(i => i.Id == id).First();
                _prodContext.Products.Remove(product);
                _prodContext.SaveChanges();

                ProductList list = new ProductList(_prodContext);
                List<Product> products = list.GetProductRange(10);
                List<Object> anonValueList = new List<Object>();

                foreach (var j in products)
                {
                    anonValueList.Add(new
                    {
                        Id = j.Id,
                        Name = j.Name,
                        Price = j.Price,
                        Count = j.Count
                    });
                }

                string jsonResult = JsonConvert.SerializeObject(anonValueList.ToArray());
                return jsonResult;
            }
        }

        [HttpPost]
        public string EditProduct()
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                int id = Convert.ToInt32(Request.Params["Id"]);
                string name = Request.Params["Name"];
                float price = Convert.ToSingle(Request.Params["Price"]);

                Product prod = _prodContext.Products.Where(i => i.Id == id).First();
                prod.Name = name;
                prod.Price = price;
                _prodContext.SaveChanges();

                ProductList list = new ProductList(_prodContext);
                List<Product> products = list.GetProductRange(10);
                List<Object> anonValueList = new List<Object>();

                foreach (var j in products)
                {
                    anonValueList.Add(new
                    {
                        Id = j.Id,
                        Name = j.Name,
                        Price = j.Price,
                        Count = j.Count
                    });
                }

                string jsonResult = JsonConvert.SerializeObject(anonValueList.ToArray());
                return jsonResult;
            }
        }
    }
}
