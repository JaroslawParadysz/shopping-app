using ShoppingApp.Modules.Products.Core.DTO;
using System.Collections.Generic;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class GetAllResponse
    {
        public IEnumerable<ProductDto> Data { get; set; }
    }
}
