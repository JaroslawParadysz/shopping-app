using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ProducttestsFixture
    {
        public IServiceCollection ServiceCollection { get; set; }
        public Action<IServiceCollection> RegisterServicesAction { get; set; }

        public ProducttestsFixture()
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
            using (var scope = ServiceCollection.BuildServiceProvider().CreateScope())
            {
                var productDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                await productDbContext.SaveChangesAsync();
            }
        }

        public void RemoveCategory(Guid categoryId)
        {
            using (var scope = ServiceCollection.BuildServiceProvider().CreateScope())
            {
                var productDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

                var categoryToRemove = productDbContext.Categories.Single(x => x.Id == categoryId);
                var productsToRemove = productDbContext.Products.Where(x => x.Category.Id == categoryId).ToList();

                productDbContext.Products.RemoveRange(productsToRemove);
                productDbContext.Categories.Remove(categoryToRemove);
                productDbContext.SaveChanges();
            }
        }
    }
}
