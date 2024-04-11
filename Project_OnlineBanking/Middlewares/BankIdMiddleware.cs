using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Project_OnlineBanking.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BankIdMiddleware
    {
        private readonly RequestDelegate _next;

        public BankIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string url = httpContext.Request.Path.ToString();
            if (url.StartsWith("/home", StringComparison.InvariantCultureIgnoreCase))
            {
                if (httpContext.Session.GetInt32("bankId") == null)
                {
                    httpContext.Response.Redirect("/account/login");
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BankIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseBankIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BankIdMiddleware>();
        }
    }
}
