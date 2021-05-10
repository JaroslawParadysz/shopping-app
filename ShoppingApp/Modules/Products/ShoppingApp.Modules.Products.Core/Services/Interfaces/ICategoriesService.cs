using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using System;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<CategoryDto> GetAsync(Guid id);
        Task<Guid> AddAsync(CategoryDto dto);
    }
}
