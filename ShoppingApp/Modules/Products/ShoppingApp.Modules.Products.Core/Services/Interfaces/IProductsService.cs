using ShoppingApp.Modules.Products.Core.DTO;
using System;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.Services.Interfaces
{
    public interface IProductsService
    {
        Task<Guid> AddAsync(ProductDto dto);
        Task<ProductDto> GetAsync(Guid id);
    }
}
