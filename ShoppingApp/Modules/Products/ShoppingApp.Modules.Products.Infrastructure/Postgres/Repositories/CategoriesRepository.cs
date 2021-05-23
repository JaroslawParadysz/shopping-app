using Microsoft.EntityFrameworkCore;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Infrastructure.Postgres.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ProductsDbContext _productsDbContext;

        public CategoriesRepository(ProductsDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        public async Task<Guid> AddAsync(Category category)
        {
            await _productsDbContext.Categories.AddAsync(category);
            await _productsDbContext.SaveChangesAsync();

            return category.Id;
        }

        public async Task DelateAsync(Guid id)
        {
            var category = await GetAsync(id);
            _productsDbContext.Categories.Remove(category);
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await _productsDbContext.Categories.SingleOrDefaultAsync(category => category.Id == id);
        }

        public async Task<IList<Category>> GetAsync()
        {
            return await _productsDbContext.Categories.ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            var toUpdate = await GetAsync(category.Id);
            toUpdate.Name = category.Name;

            await _productsDbContext.SaveChangesAsync();
        }
    }
}
