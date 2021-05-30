using Mapster;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Exceptions;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public ProductsService(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
        {
            _productsRepository = productsRepository;
            _categoriesRepository = categoriesRepository;
        }

        public async Task<Guid> AddAsync(ProductDto dto)
        {
            var category = await _categoriesRepository.GetAsync(dto.CategoryId);
            Product product = dto.Adapt<Product>();
            product.Category = category;
            return await _productsRepository.AddAsync(product);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _productsRepository.GetAsync(id);

            if (product is null)
            {
                throw new ProductNotFoundException(id);
            }

            var dto = product.Adapt<ProductDto>();

            return dto;
        }
    }
}
