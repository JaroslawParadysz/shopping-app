using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;

namespace ShoppingApp.Modules.Products.Api
{
    public static class ProductsModule
    {
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            services.AddProductsDbContext();
            return services;
        }
    }
}
