using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class Create : BaseAsyncEndpoint.WithRequest<CreateCategoryCommand>.WithoutResponse
    {
        [HttpPost(ProductsRest.CategoriesPath)]
        public async override Task<ActionResult> HandleAsync([FromBody] CreateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return CreatedAtRoute("", new { Id = 1 }, null);
        }
    }
}
