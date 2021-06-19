using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class Delete : BaseAsyncEndpoint.WithRequest<Guid>.WithoutResponse
    {
        private readonly IProductsService _productsService;

        public Delete(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpDelete(ProductsRest.ProductsPath + "/{id}", Name = "DeleteProduct")]
        public async override Task<ActionResult> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            await _productsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
