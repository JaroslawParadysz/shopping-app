using Microsoft.AspNetCore.Mvc;
using System;

namespace ShoppingApp.Modules.Products.Api.Endpoints.Categories
{
    public class UpdateCategoryCommand
    {
        public string Name { get; set; }
    }
}
