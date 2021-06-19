using System;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class UpdateProductCommand
    {
        public string ProductName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
