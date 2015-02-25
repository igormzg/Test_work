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
        ProductsContext _ProdContext = new ProductsContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            _ProdContext.Products.Add(product);
            _ProdContext.SaveChanges();
            return View("Index");
        }

        public string GetProducts()
        {
            List<Object> anonValueList = new List<object>();

            List<Product> products = _ProdContext.Products.ToList();

            int index = (Convert.ToInt32(Request.Params["page"]) - 1) * 10;
            if ((products.Count() - index) >= 10)
            {
                products = products.GetRange(index, 10);
            }
            else
            {
                products = products.GetRange(index, products.Count() - index);
            }

            foreach (var j in products)
            {
                anonValueList.Add(new
                {
                    Name = j.Name,
                    Price = j.Price
                });
            }

            string jsonResult = JsonConvert.SerializeObject(anonValueList.ToArray());
            return jsonResult;

        }
    }
}
