using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Products_grid.Models.Products
{
    public partial class Operation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public int Count { get; set; }
        public System.DateTime Date { get; set; }

        public virtual Product Products { get; set; }
        public virtual OperationType OperationType { get; set; }
    }

    public partial class OperationType
    {
        public OperationType()
        {
            this.Operations = new HashSet<Operation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }

    public partial class Product
    {
        public Product()
        {
            this.Operations = new HashSet<Operation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }

    public partial class ProductsEntities : DbContext
    {
        public ProductsEntities()
            : base("name=ProductsEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationType> OperationType { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}