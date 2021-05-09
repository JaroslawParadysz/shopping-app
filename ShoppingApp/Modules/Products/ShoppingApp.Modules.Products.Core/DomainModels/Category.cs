using System;
using System.Collections.Generic;

namespace ShoppingApp.Modules.Products.Core.DomainModels
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Product> Products { get; set; }
    }
}
