using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class Create : BaseAsyncEndpoint.WithRequest<CreateCategoryCommand>.WithoutResponse
    {
        private readonly ICategoriesService _categoriesService;
        public Create(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpPost(ProductsRest.CategoriesPath)]
        public async override Task<ActionResult> HandleAsync([FromBody] CreateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            var category = request.Adapt<CategoryDto>();
            var categoryId = await _categoriesService.AddAsync(category);
            return CreatedAtRoute("GetCategory", new { Id = categoryId }, null);
        }
    }
}
