using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Core.DAL;
using ShoppingApp.Modules.Products.Infrastructure.Postgres.Repositories;
using ShoppingApp.Shared.Infrastructure.Postgres;

namespace ShoppingApp.Modules.Products.Infrastructure.Postgres
{
    public static class Extensions
    {
        public static IServiceCollection AddProductsDbContext(this IServiceCollection services)
        {
            services.AddPostgresDbContext<ProductsDbContext>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IProductsRepository, ProductsReporistory>();
            return services;
        }
    }
}
