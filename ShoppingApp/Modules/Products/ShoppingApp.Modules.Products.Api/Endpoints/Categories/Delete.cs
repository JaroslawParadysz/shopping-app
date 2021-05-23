using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class Delete : BaseAsyncEndpoint.WithRequest<Guid>.WithoutResponse
    {
        private readonly ICategoriesService _categoriesService;

        public Delete(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpDelete(ProductsRest.CategoriesPath + "/{id}", Name = "Delete")]
        public override async Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _categoriesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
