using System;

namespace ShoppingApp.Modules.Products.Core.DomainModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
