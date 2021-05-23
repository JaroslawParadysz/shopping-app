using System;

namespace ShoppingApp.Shared.Abstraction
{
    public class ShoppingAppException : Exception
    {
        public ShoppingAppException(string message) : base(message)
        {

        }
    }
}
