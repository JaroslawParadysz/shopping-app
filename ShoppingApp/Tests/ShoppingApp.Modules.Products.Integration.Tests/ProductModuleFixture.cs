using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ProductModuleFixture
    {
        public IServiceCollection ServiceCollection { get; set; }
        public Action<IServiceCollection> RegisterServicesAction { get; set; }

        public ProductModuleFixture()
        {
            RegisterServicesAction = (svc) => {
                var serviceDescriptor = svc.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductsDbContext>));
                svc.Remove(serviceDescriptor);
                svc.AddDbContext<ProductsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));
                ServiceCollection = svc;
            };
        }

        public async Task AddCategoryAsync(Category category)
        {
            using (ServiceProvider serviceProvider = ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                await productDbContext.SaveChangesAsync();
            }
        }

        public void CleanDatabase()
        {
            using (ServiceProvider serviceProvider = ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Products.RemoveRange(productDbContext.Products.ToList());
                productDbContext.Categories.RemoveRange(productDbContext.Categories.ToList());
                productDbContext.SaveChanges();
            }
        }
    }
}
