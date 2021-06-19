using Mapster;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Exceptions;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            Category category = await _categoriesRepository.GetAsync(dto.CategoryId);
            if (category is null) throw new CategoryNotFoundException(dto.CategoryId);

            Product product = dto.Adapt<Product>();
            product.Category = category;
            return await _productsRepository.AddAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productsRepository.GetAsync(id);

            if (product is null) throw new ProductNotFoundException(id);

            await _productsRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _productsRepository.GetAsync(id);

            if (product is null) throw new ProductNotFoundException(id);

            var dto = product.Adapt<ProductDto>();

            return dto;
        }

        public async Task<IEnumerable<ProductDto>> GetAsync()
        {
            var products = await _productsRepository.GetAsync();
            var dtos = products.Adapt<IEnumerable<ProductDto>>();

            return dtos;
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            Product product = await _productsRepository.GetAsync(dto.Id);
            if (product is null) throw new ProductNotFoundException(dto.Id);

            Category category = await _categoriesRepository.GetAsync(dto.CategoryId);
            if (category is null) throw new CategoryNotFoundException(dto.CategoryId);

            product.Name = dto.Name;
            product.Category = category;

            await _productsRepository.UpdateAsync(product);
        }
    }
}
