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
using System.Threading.Tasks;
using Xunit;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ServicesFixture
    {
        public ServicesFixture()
        {
            var cat = new Core.DomainModels.Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            RegisterServicesAction = (svc) => {
                var serviceDescriptor = svc.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductsDbContext>));
                svc.Remove(serviceDescriptor);
                svc.AddDbContext<ProductsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));

                var sp = svc.BuildServiceProvider();
                var dbContext = sp.GetService<ProductsDbContext>();
                dbContext.Categories.Add(cat);
                dbContext.SaveChanges();
            };
        }

        public Action<IServiceCollection> RegisterServicesAction { get; set; }
    }

    public class ProductsTests : IClassFixture<ServicesFixture>
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

            var response = await client.GetAsync(ProductsRest.ProductsPath);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = JsonConvert.DeserializeObject<GetAllResponse>(await response.Content.ReadAsStringAsync());
            payload.Data.Count().Should().Be(1);
            payload.Data.Single().Name.Should().Be("Abc Product");
        }

        [Fact]
        public async Task Get_Expect_BadRequest()
        {
            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);

            var client = factory.CreateClient();
            var unexistingProductId = Guid.NewGuid();
            var response = await client.GetAsync(ProductsRest.ProductsPath + "/" + unexistingProductId);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var payload = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
            payload.Code.Should().Be("product_not_found");
            payload.Message.Should().Be($"Product with id {unexistingProductId} has not been found.");
        }
    }
}
