using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<CategoryDto> GetAsync(Guid id);
        Task<IList<CategoryDto>> GetAsync();
        Task<Guid> AddAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto categoryDto);
    }
}
