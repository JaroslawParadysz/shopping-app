using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Shared.Infrastructure.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Shared.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddControllers().ConfigureApplicationPartManager(
                appPartManager => appPartManager.FeatureProviders.Add(new InternalControllerFeatureProvider())
            );

            return services;
        }
    }
}
