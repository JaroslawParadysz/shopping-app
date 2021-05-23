using Humanizer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShoppingApp.Shared.Abstraction;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ShoppingApp.Bootstraper.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private ConcurrentDictionary<string, string> _codes = new ConcurrentDictionary<string, string>();

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string message = "There was an error.";
                string code = "error";
                int statusCode = 500;

                if (ex is ShoppingAppException)
                {
                    if (!_codes.TryGetValue(ex.GetType().Name, out code))
                    {
                        code = ex.GetType().Name.Underscore().Replace("_exception", string.Empty);
                        _codes.TryAdd(ex.GetType().Name, code);
                    }

                    statusCode = StatusCodes.Status400BadRequest;
                    
                    message = ex.Message;
                }

                context.Response.StatusCode = statusCode;
                var error = new Error() 
                { 
                    Message = message,
                    Code = code
                };
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
