using Ardalis.ApiEndpoints;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Products
{
    public class GetAll : BaseAsyncEndpoint.WithoutRequest.WithResponse<GetAllResponse>
    {
        private readonly IProductsService _productsService;

        public GetAll(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet(ProductsRest.ProductsPath)]
        public override async Task<ActionResult<GetAllResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.CategoryId, src => src.Category.Id);

            var dtos = await _productsService.GetAsync();
            return new GetAllResponse { Data = dtos };
        }
    }
}
