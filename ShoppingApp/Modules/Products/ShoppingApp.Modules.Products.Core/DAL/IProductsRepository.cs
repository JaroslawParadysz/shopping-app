using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.DAL
{
    public interface IProductsRepository
    {
        Task<Guid> AddAsync(Product product);
        Task<Product> GetAsync(Guid id);
        Task<IEnumerable<Product>> GetAsync();
    }
}
