using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.DAL
{
    public interface ICategoriesRepository
    {
        Task<Guid> AddAsync(Category category);
        Task<Category> GetAsync(Guid id);
        Task<IList<Category>> GetAsync();
        Task UpdateAsync(Category category);
        Task DelateCategory(Guid id);
    }
}
