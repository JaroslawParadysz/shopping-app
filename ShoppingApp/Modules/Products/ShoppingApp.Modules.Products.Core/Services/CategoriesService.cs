using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Exceptions;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
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

            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<Guid> AddAsync(CategoryDto dto)
        {
            var category = new Category { Name = dto.Name };
            return await _repository.AddAsync(category);
        }
    }
}
