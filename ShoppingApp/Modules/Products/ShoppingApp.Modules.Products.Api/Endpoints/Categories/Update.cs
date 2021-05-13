using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Modules.Products.Core.DTO;
using ShoppingApp.Modules.Products.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class Update : BaseAsyncEndpoint.WithRequest<UpdateCategoryCommand>.WithoutResponse
    {
        private readonly ICategoriesService _categoriesService;

        public Update(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpPut(ProductsRest.CategoriesPath  + "/{id}")]
        public override async Task<ActionResult> HandleAsync([FromBody] UpdateCategoryCommand request, CancellationToken cancellationToken = default)
        {
            if (!Request.RouteValues.ContainsKey("id")) throw new ArgumentException("Id cannot be a null");

            var id = Guid.Parse(Request.RouteValues["id"].ToString());
            await _categoriesService.UpdateAsync(new CategoryDto { Id = id, Name = request.Name });
            return NoContent();
        }
    }
}
