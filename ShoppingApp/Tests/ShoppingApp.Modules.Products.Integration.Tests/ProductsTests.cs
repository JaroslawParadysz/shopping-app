using FluentAssertions;
using ShoppingApp.Bootstraper.Middlewares;
using ShoppingApp.Modules.Products.Api.Endpoints;
using ShoppingApp.Modules.Products.Api.Endpoints.Products;
using ShoppingApp.Modules.Products.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingApp.Modules.Products.Integration.Tests
{
    public class ProductsTests : IClassFixture<ProducttestsFixture>
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
                Name = "Abc Category get",
                Products = new List<Product> { new Product { Name = "Abc Product get" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            //Act
            var response = await client.GetAsync(ProductsRest.ProductsPath);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = JsonSerializer.Deserialize<GetAllResponse>(await response.Content.ReadAsStringAsync());
            payload.Data.Count().Should().Be(1);
            payload.Data.Single().Name.Should().Be("Abc Product get");

            ServicesFixture.RemoveCategory(category.Id);
        }

        [Fact]
        public async Task Get_For_Unexisting_ProductId_Expects_BadRequest()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category not found",
                Products = new List<Product> { new Product { Name = "Abc Product not found" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            var unexistingProductId = Guid.NewGuid();

            //Act
            var response = await client.GetAsync(ProductsRest.ProductsPath + "/" + unexistingProductId);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var payload = JsonSerializer.Deserialize<Error>(await response.Content.ReadAsStringAsync());
            payload.Code.Should().Be("product_not_found");
            payload.Message.Should().Be($"Product with id {unexistingProductId} has not been found.");

            ServicesFixture.RemoveCategory(category.Id);
        }

        [Fact]
        public async Task Create_Expects_Success()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category create"
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            CreateProductCommand command = new CreateProductCommand { CategoryId = category.Id, ProductName = "Probuct Abc" };
            var json = JsonSerializer.Serialize(command);

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync(ProductsRest.ProductsPath, content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            ServicesFixture.RemoveCategory(category.Id);
        }
            
        [Fact]
        public async Task Update_Expects_NoContent_StatusCode()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category Update",
                Products = new List<Product> { new Product { Name = "Abc Product Update" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);
            UpdateProductCommand command = new UpdateProductCommand { CategoryId = category.Id, ProductName = "Updated product name" };
            string json = JsonSerializer.Serialize(command);
            StringContent payload = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"{ProductsRest.ProductsPath}/{category.Products.Single().Id}", payload);

            //Asset
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            ServicesFixture.RemoveCategory(category.Id);
        }

        [Fact]
        public async Task Delete_Expect_NoContent_Statuscode()
        {
            //Arrange
            Category category = new Category
            {
                Name = "Abc Category delete",
                Products = new List<Product> { new Product { Name = "Abc Product delete" } }
            };

            var factory = new CustomWebApplicationFactory<Bootstraper.Startup>(ServicesFixture.RegisterServicesAction);
            var client = factory.CreateClient();
            await ServicesFixture.AddCategoryAsync(category);

            //Act
            var response = await client.DeleteAsync($"{ProductsRest.ProductsPath}/{category.Products.Single().Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            ServicesFixture.RemoveCategory(category.Id);
        }
    }
}
