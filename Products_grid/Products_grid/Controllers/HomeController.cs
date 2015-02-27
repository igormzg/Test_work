using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products_grid.Models.Products;
using Newtonsoft.Json;
using Products_grid.Models;

namespace Products_grid.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string AddProduct(Product product)
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                _prodContext.Products.Add(product);
                _prodContext.SaveChanges();
                return "";
            }
        }

        public string GetProducts()
        {
            using (ProductsContext _prodContext = new ProductsContext())
            {
                ProductPager list = new ProductPager(_prodContext);
                int rowCount = 10;
                int firstIndex = (Convert.ToInt32(Request.Params["page"]) - 1) * rowCount;
                List<Product> products = list.GetPage(firstIndex, rowCount);
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

                ProductPager list = new ProductPager(_prodContext);
                List<Product> products = list.GetPage(10);
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
                int id = Convert.ToInt32(Request.Params["ProductId"]);
                string name = Request.Params["Name"];
                float price = Convert.ToSingle(Request.Params["Price"]);

                Product prod = _prodContext.Products.Where(i => i.Id == id).First();
                prod.Name = name;
                prod.Price = price;
                _prodContext.SaveChanges();

                ProductPager list = new ProductPager(_prodContext);
                List<Product> products = list.GetPage(10);
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
        public ActionResult AddOperation()
        {
            using (UsersContext _userContext = new UsersContext())
            using (ProductsContext _prodContext = new ProductsContext())
            {
                int id = Convert.ToInt32(Request.Params["id"]);
                int count = Convert.ToInt32(Request.Params["count"]);
                OperationType opType;
                if(Convert.ToBoolean(Request.Params["type"]))
                {
                    opType= OperationType.plus;
                }
                else 
                {
                    opType = OperationType.minus;
                }

                ////
                //Доработать, теперь передается контекст а не юзер
                ////
                int userId = (from a in _userContext.UserProfiles where a.UserName == User.Identity.Name select a.UserId).First();
                OperationManager oper = new OperationManager(_prodContext, _userContext);
                oper.AddOperation(opType, count, id, userId);

                return new EmptyResult();
            }
        }

        public string GetOperations()
        {
            using (UsersContext _userContext = new UsersContext())
            using (ProductsContext _prodContext = new ProductsContext())
            {
                OperationManager manager = new OperationManager(_prodContext, _userContext);
                int rowCount = 10;
                int firstIndex;
                int prodId;
                try 
                {
                    rowCount = Convert.ToInt32(Request.Params["rows"]);
                    firstIndex = (Convert.ToInt32(Request.Params["page"]) - 1) * rowCount;
                    prodId = Convert.ToInt32(Request.Params["prodId"]);
                }
                catch(Exception ex)
                {
                    return "";
                }

                List<OperationRow> resultList;
                resultList = manager.GetPage(firstIndex, rowCount, prodId);
                if (resultList == null) return "";
                if (Request.Params["sidx"] != null)
                    resultList = manager.Sort(resultList, Request.Params["sidx"], Request.Params["sord"]);

                string jsonResult = JsonConvert.SerializeObject(resultList.ToArray());
                return jsonResult;
            }
        }
    }
}
