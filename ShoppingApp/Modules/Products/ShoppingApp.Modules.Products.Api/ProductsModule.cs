using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Api.Endpoints.Products;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Services;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;

namespace ShoppingApp.Modules.Products.Api
{
    public static class ProductsModule
    {
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            services.AddProductsDbContext();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();

            return services;
        }

        public static void ConfigureTypesMappings()
        {
            TypeAdapterConfig<ProductDto, GetProductResponse>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Name)
                .Map(dest => dest.ProductId, src => src.Id);

            TypeAdapterConfig<Product, ProductDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.CategoryId, src => src.Category.Id);
        }
    }
}
