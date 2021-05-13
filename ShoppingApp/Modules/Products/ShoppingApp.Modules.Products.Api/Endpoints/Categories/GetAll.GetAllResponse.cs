using ShoppingApp.Modules.Products.Core.DTO;
using System.Collections.Generic;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class GetAllResponse
    {
        public IList<CategoryDto> Categories { get; set; }
    }
}
