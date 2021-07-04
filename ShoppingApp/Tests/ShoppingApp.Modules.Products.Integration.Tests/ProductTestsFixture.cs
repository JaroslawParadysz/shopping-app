using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ProductTestsFixture
    {
        private IServiceCollection _serviceCollection;
        public Action<IServiceCollection> RegisterServicesAction { get; set; }

        public ProductTestsFixture()
        {
            RegisterServicesAction = (svc) => {
                var serviceDescriptor = svc.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductsDbContext>));
                svc.Remove(serviceDescriptor);
                svc.AddDbContext<ProductsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));
                _serviceCollection = svc;
            };
        }

        public async Task AddCategoryAsync(Category category)
        {
            using (var scope = _serviceCollection.BuildServiceProvider().CreateScope())
            {
                var productDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                await productDbContext.SaveChangesAsync();
            }
        }

        public void RemoveCategory(Guid categoryId)
        {
            using (var scope = _serviceCollection.BuildServiceProvider().CreateScope())
            {
                var productDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

                var categoryToRemove = productDbContext.Categories.Single(x => x.Id == categoryId);
                var productsToRemove = productDbContext.Products.Where(x => x.Category.Id == categoryId).ToList();

                productDbContext.Products.RemoveRange(productsToRemove);
                productDbContext.Categories.Remove(categoryToRemove);
                productDbContext.SaveChanges();
            }
        }

        public void RemoveCategory()
        {
            using (var scope = _serviceCollection.BuildServiceProvider().CreateScope())
            {
                var productDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

                var categoryToRemove = productDbContext.Categories.ToList();
                var productsToRemove = productDbContext.Products.ToList();

                productDbContext.Products.RemoveRange(productsToRemove);
                productDbContext.Categories.RemoveRange(categoryToRemove);
                productDbContext.SaveChanges();
            }
        }
    }
}
