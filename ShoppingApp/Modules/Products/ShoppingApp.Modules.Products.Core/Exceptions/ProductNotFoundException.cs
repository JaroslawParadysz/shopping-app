using ShoppingApp.Shared.Abstraction;
using System;

namespace ShoppingApp.Modules.Products.Core.Exceptions
{
    public class ProductNotFoundException : ShoppingAppException
    {
        public ProductNotFoundException(Guid id) : base($"Product with id {id} has not been found.")
        {
        }
    }
}
