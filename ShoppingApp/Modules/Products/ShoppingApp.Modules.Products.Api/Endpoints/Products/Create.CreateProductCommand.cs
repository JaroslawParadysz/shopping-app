using System;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class CreateProductCommand
    {
        public string ProductName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
