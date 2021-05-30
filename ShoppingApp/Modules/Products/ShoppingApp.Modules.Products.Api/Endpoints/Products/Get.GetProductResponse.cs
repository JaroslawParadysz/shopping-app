using System;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class GetProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
