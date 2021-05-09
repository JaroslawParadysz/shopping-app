using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Shared.Infrastructure.Api;
using ShoppingApp.Shared.Infrastructure.Postgres;

namespace ShoppingApp.Shared.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddControllers().ConfigureApplicationPartManager(
                appPartManager => appPartManager.FeatureProviders.Add(new InternalControllerFeatureProvider())
            );
            services.AddPostgres();

            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var sp = services.BuildServiceProvider();
            T options = new T();
            var section = sp.GetRequiredService<IConfiguration>().GetSection(sectionName);
            section.Bind(options);

            return options;
        }
    }
}
