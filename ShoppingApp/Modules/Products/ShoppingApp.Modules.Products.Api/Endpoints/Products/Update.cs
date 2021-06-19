using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class Update : BaseAsyncEndpoint.WithRequest<UpdateProductCommand>.WithoutResponse
    {
        private readonly IProductsService _productsService;

        public Update(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPut(ProductsRest.ProductsPath + "/{id}")]
        public async override Task<ActionResult> HandleAsync([FromBody]UpdateProductCommand request, CancellationToken cancellationToken = default)
        {
            if (!Request.RouteValues.ContainsKey("id")) throw new ArgumentException("Id cannot be a null");
            var productId = Guid.Parse(Request.RouteValues["id"].ToString());

            await _productsService.UpdateAsync(new Core.DTO.ProductDto { Id = productId, CategoryId = request.CategoryId, Name = request.ProductName });

            return NoContent();
        }
    }
}
