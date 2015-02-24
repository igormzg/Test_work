using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products_grid.Models.Products
{
    public class Operation
    {
        public int id { get; set; }

        public Product product_id { get; set; }

        public OperationType operation_type_id { get; set; }

        public int count { get; set; }

        public DateTime date { get; set; }
    }
}