using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Action<IServiceCollection> Registration { get; set; }

        public CustomWebApplicationFactory() : this(null)
        {

        }

        public CustomWebApplicationFactory(Action<IServiceCollection> registration)
        {
            Registration = registration ?? (collections => { });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                Registration?.Invoke(services);
            });
        }
    }
}
