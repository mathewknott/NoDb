using Mat.Web.Helpers;
using Mat.Web.Middleware;
using Mat.Web.Models;
using Microsoft.AspNetCore.Builder;

namespace Mat.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
        {
            SecurityHeadersPolicy policy = builder.Build();
            return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
        }
    }
}