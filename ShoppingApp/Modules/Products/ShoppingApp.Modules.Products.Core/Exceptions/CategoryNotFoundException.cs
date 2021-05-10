using System;

namespace ShoppingApp.Modules.Products.Core.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(Guid id) : base($"Category with id {id} has not bee found.")
        {
        }
    }
}
