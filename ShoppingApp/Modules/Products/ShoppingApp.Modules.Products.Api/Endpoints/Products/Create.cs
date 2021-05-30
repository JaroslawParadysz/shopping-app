using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class Create : BaseAsyncEndpoint.WithRequest<CreateProductCommand>.WithoutResponse
    {
        private readonly IProductsService _productsService;

        public Create(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPost(ProductsRest.ProductsPath)]
        public override async Task<ActionResult> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken = default)
        {
            TypeAdapterConfig<CreateProductCommand, ProductDto>.NewConfig()
                .Map(dest => dest.Name, src => src.ProductName);
            TypeAdapterConfig<ProductDto, CreateProductCommand>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Name);

            var dto = request.Adapt<ProductDto>();
            var productId = await _productsService.AddAsync(dto);

            return CreatedAtRoute("GetProduct", new { id = productId }, null);
        }
    }
}
