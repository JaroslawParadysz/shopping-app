using ShoppingApp.Shared.Abstraction;
using System;

namespace ShoppingApp.Modules.Products.Core.Exceptions
{
    public class CategoryNotFoundException : ShoppingAppException
    {
        public CategoryNotFoundException(Guid id) : base($"Category with id {id} has not been found.")
        {
        }
    }
}
