using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class GetAll : BaseAsyncEndpoint.WithoutRequest.WithResponse<GetAllResponse>
    {
        private readonly ICategoriesService _categoriesService;

        public GetAll(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet(ProductsRest.CategoriesPath)]
        public override async Task<ActionResult<GetAllResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var categoryDtos = await _categoriesService.GetAsync();
            return new GetAllResponse { Categories = categoryDtos };
        }
    }
}
