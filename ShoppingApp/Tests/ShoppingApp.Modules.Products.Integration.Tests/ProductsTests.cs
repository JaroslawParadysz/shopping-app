using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShoppingApp.Bootstraper.Middlewares;
using ShoppingApp.Modules.Products.Api.Endpoints;
using ShoppingApp.Modules.Products.Api.Endpoints.Products;
using ShoppingApp.Modules.Products.Core.DomainModels;
using ShoppingApp.Modules.Products.Infrastructure.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ServicesFixture : IDisposable
    {
        public IServiceCollection ServiceCollection { get; set; }

        public ServicesFixture()
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

        public Action<IServiceCollection> RegisterServicesAction { get; set; }

        public void Dispose()
        {
            int i = 10;
        }
    }

    public class ProductsTests : IClassFixture<ServicesFixture>, IDisposable
    {
        public ProductsTests(ServicesFixture servicesFixture)
        {
            ServicesFixture = servicesFixture;
        }

        public ServicesFixture ServicesFixture { get; set; }

        [Fact]
        public async Task GetAsync_Expect_Success()
        {            
            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();

            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            using (ServiceProvider serviceProvider = ServicesFixture.ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                productDbContext.SaveChanges();
            }

            var response = await client.GetAsync(ProductsRest.ProductsPath);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = JsonConvert.DeserializeObject<GetAllResponse>(await response.Content.ReadAsStringAsync());
            payload.Data.Count().Should().Be(1);
            payload.Data.Single().Name.Should().Be("Abc Product");
        }

        [Fact]
        public async Task Get_For_Unexisting_ProductId_Expects_BadRequest()
        {
            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();

            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            using (ServiceProvider serviceProvider = ServicesFixture.ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                productDbContext.SaveChanges();
            }

            var unexistingProductId = Guid.NewGuid();
            var response = await client.GetAsync(ProductsRest.ProductsPath + "/" + unexistingProductId);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var payload = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
            payload.Code.Should().Be("product_not_found");
            payload.Message.Should().Be($"Product with id {unexistingProductId} has not been found.");
        }

        [Fact]
        public async Task Create_Expects_Success()
        {
            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();

            Category category = new Category
            {
                Name = "Abc Category"
            };

            using (ServiceProvider serviceProvider = ServicesFixture.ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Categories.Add(category);
                productDbContext.SaveChanges();
            }

            CreateProductCommand command = new CreateProductCommand { CategoryId = category.Id, ProductName = "Probuct Abc" };
            var json = JsonConvert.SerializeObject(command);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ProductsRest.ProductsPath, content);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        public void Dispose()
        {
            using (ServiceProvider serviceProvider = ServicesFixture.ServiceCollection.BuildServiceProvider())
            {
                var productDbContext = serviceProvider.GetRequiredService<ProductsDbContext>();
                productDbContext.Products.RemoveRange(productDbContext.Products.ToList());
                productDbContext.Categories.RemoveRange(productDbContext.Categories.ToList());
                productDbContext.SaveChanges();
            }
        }
    }
}
