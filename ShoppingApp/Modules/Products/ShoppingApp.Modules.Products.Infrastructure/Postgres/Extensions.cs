using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Shared.Infrastructure.Postgres;

namespace ShoppingApp.Modules.Products.Infrastructure.Postgres
{
    public static class Extensions
    {
        public static IServiceCollection AddProductsDbContext(this IServiceCollection services)
        {
            services.AddPostgresDbContext<ProductsDbContext>();
            return services;
        }
    }
}
