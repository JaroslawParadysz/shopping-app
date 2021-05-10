﻿using Microsoft.EntityFrameworkCore;
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

        public Task DelateCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await _productsDbContext.Categories.SingleOrDefaultAsync(category => category.Id == id);
        }

        public Task<IList<Category>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
