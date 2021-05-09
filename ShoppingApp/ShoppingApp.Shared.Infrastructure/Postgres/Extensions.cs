using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingApp.Shared.Infrastructure.Postgres
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var postgresOptions = services.GetOptions<PostgresOptions>("postgres");
            services.AddSingleton(postgresOptions);

            return services;
        }

        public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            var postgresOptions = services.GetOptions<PostgresOptions>("postgres");
            services.AddDbContext<TDbContext>(x => x.UseNpgsql(postgresOptions.ConnectionString));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            dbContext.Database.Migrate();

            return services;
        }
    }
}
