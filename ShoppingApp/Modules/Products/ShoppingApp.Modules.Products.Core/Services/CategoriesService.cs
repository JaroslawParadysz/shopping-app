using Mapster;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Exceptions;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _repository;

        public CategoriesService(ICategoriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _repository.GetAsync(id);
            if (category is null) throw new CategoryNotFoundException(id);

            return category.Adapt<CategoryDto>();
        }

        public async Task<Guid> AddAsync(CategoryDto dto)
        {
            var category = dto.Adapt<Category>();
            return await _repository.AddAsync(category);
        }

        public async Task<IList<CategoryDto>> GetAsync()
        {
            var categories = await _repository.GetAsync();
            var categoryDtos = categories.Select(category => category.Adapt<CategoryDto>())
                .ToList();

            return categoryDtos;
        }

        public async Task UpdateAsync(CategoryDto categoryDto)
        {
            var category = categoryDto.Adapt<Category>();
            await _repository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DelateAsync(id);
        }
    }
}
