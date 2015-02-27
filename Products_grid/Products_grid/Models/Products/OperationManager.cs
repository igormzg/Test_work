using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products_grid.Models.Products
{
    public class OperationManager
    {
        ProductsContext _productContext;

        UsersContext _userContext;

        public OperationManager(ProductsContext productContext, UsersContext userContext)
        {
            _productContext = productContext;
            _userContext = userContext;
        }

        public bool AddOperation(OperationType type, int count, int productId, int userId)
        {
            Product prod = _productContext.Products.Where(i => productId == i.Id).FirstOrDefault();
            if (prod == null) return false;

            if (type == OperationType.plus)
            {
                prod.Count += count;
                Operation oper = new Operation(){
                    Date = DateTime.Now,
                    Count = count,
                    ProductId = productId,
                    Type = true,
                    UserId = userId
                };
                _productContext.Operations.Add(oper);
            }
            else
            {
                if (prod.Count < count) return false;
                prod.Count -= count;
                Operation oper = new Operation()
                {
                    Date = DateTime.Now,
                    Count = count,
                    ProductId = productId,
                    Type = false,
                    UserId = userId
                };
                _productContext.SaveChanges();
            }

            _productContext.SaveChanges();
            return true;
        }

        public List<OperationRow> GetPage(int firstIndex, int rows, int prodId)
        {
            List<Operation> operations = _productContext.Operations.Where(x => x.ProductId == prodId).ToList();

            if ((operations.Count() - firstIndex) >= rows)
            {
                operations = operations.GetRange(firstIndex, rows);
            }
            else
            {
                operations = operations.GetRange(firstIndex, operations.Count() - firstIndex);
            }

            List<OperationRow> resultList = new List<OperationRow>();

            foreach (var i in operations)
            {
                UserProfile user = _userContext.UserProfiles.Where(x => x.UserId == i.UserId).FirstOrDefault();
                if (user == null) return null;
                Product prod = _productContext.Products.Where(x => x.Id == i.ProductId).FirstOrDefault();
                if (prod == null) return null;

                resultList.Add(new OperationRow
                {
                    Id = i.Id,
                    User = user.UserName,
                    Product = prod.Name,
                    Type = i.Type,
                    Count = i.Count,
                    Date = i.Date
                });
            }

            return resultList;
        }

        public List<OperationRow> GetPage(int rows, int prodId)
        {
            return GetPage(0, rows, prodId);
        }

        public List<OperationRow> Sort(List<OperationRow> list, string sortField, string sotrType)
        {
            if (sortField == "Id")
            {
                list.Sort((x1, x2) => x1.Id.CompareTo(x2.Id));
            }
            else if(sortField == "User")
            {
                list.Sort((x1, x2) => x1.User.CompareTo(x2.User));
            }
            else if(sortField == "Product")
            {
                list.Sort((x1, x2) => x1.Product.CompareTo(x2.Product));
            }
            else if(sortField == "Type")
            {
                list.Sort((x1, x2) => x1.Type.CompareTo(x2.Type));
            }
            else if(sortField == "Count")
            {
                list.Sort((x1, x2) => x1.Count.CompareTo(x2.Count));
            }
            else if (sortField == "Date")
            {
                list.Sort((x1, x2) => x1.Date.CompareTo(x2.Date));
            }

            if(sotrType == "asc")
            {
                list.Reverse();
            }
            return list;
        }

    }
}