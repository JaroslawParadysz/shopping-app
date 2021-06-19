using Microsoft.EntityFrameworkCore;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Infrastructure.Postgres.Repositories
{
    public class ProductsReporistory : IProductsRepository
    {
        private readonly ProductsDbContext _dbContext;

        public ProductsReporistory(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Product> GetAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _dbContext.Products
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
