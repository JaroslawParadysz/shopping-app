using FluentAssertions;
using Newtonsoft.Json;
using ShoppingApp.Bootstraper.Middlewares;
using ShoppingApp.Modules.Products.Api.Endpoints;
using ShoppingApp.Modules.Products.Api.Endpoints.Products;
using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ProductsTests : IClassFixture<ProducttestsFixture>, IDisposable
    {
        public ProductsTests(ProducttestsFixture servicesFixture)
        {
            ServicesFixture = servicesFixture;
        }

        public ProducttestsFixture ServicesFixture { get; set; }

        [Fact]
        public async Task GetAsync_Expect_Success()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            //Act
            var response = await client.GetAsync(ProductsRest.ProductsPath);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = JsonConvert.DeserializeObject<GetAllResponse>(await response.Content.ReadAsStringAsync());
            payload.Data.Count().Should().Be(1);
            payload.Data.Single().Name.Should().Be("Abc Product");
        }

        [Fact]
        public async Task Get_For_Unexisting_ProductId_Expects_BadRequest()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            var unexistingProductId = Guid.NewGuid();

            //Act
            var response = await client.GetAsync(ProductsRest.ProductsPath + "/" + unexistingProductId);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var payload = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
            payload.Code.Should().Be("product_not_found");
            payload.Message.Should().Be($"Product with id {unexistingProductId} has not been found.");
        }

        [Fact]
        public async Task Create_Expects_Success()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category"
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            CreateProductCommand command = new CreateProductCommand { CategoryId = category.Id, ProductName = "Probuct Abc" };
            var json = JsonConvert.SerializeObject(command);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(ProductsRest.ProductsPath, content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Update_Expects_NoContent_StatusCode()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);
            UpdateProductCommand command = new UpdateProductCommand { CategoryId = category.Id, ProductName = "Updated product name" };
            string json = JsonConvert.SerializeObject(command);
            StringContent payload = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"{ProductsRest.ProductsPath}/{category.Products.Single().Id}", payload);

            //Asset
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public async Task Delete_Expect_NoContent_Statuscode()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category",
                Products = new List<Product> { new Product { Name = "Abc Product" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();

            //Act
            var response = await client.DeleteAsync($"{ProductsRest.ProductsPath}/{category.Products.Single().Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public void Dispose()
        {
            ServicesFixture.CleanDatabase();
        }
    }
}
