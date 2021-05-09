using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.DAL
{
    internal interface ICategoriesRepository
    {
        Guid AddAsync(Category category);
        Category GetAsync(Guid id);
        IList<Category> GetAsync();
        Task UpdateAsync(Category category);
        Task DelateCategory(Guid id);
    }
}
