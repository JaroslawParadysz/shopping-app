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
    public class Get : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<GetProductResponse>
    {
        private readonly IProductsService _productsService;

        public Get(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet(ProductsRest.ProductsPath + "/{id}", Name = "GetProduct")]
        public async override Task<ActionResult<GetProductResponse>> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            TypeAdapterConfig<ProductDto, GetProductResponse>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Name)
                .Map(dest => dest.ProductId, src => src.Id);

            var dto = await _productsService.GetAsync(id);
            var response = dto.Adapt<GetProductResponse>();

            return response;
        }
    }
}
