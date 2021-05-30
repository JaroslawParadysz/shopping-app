using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class Get : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<CategoryResponse>
    {
        private readonly ICategoriesService _categoriesService;

        public Get(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet(ProductsRest.CategoriesPath + "/{id}", Name = "GetCategory")]
        public override async Task<ActionResult<CategoryResponse>> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoriesService.GetAsync(id);
            var response = new CategoryResponse { CategoryDto = category };
            return Ok(response);
        }
    }
}
