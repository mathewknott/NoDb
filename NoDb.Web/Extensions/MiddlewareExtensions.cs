using Microsoft.AspNetCore.Builder;
using NoDb.Web.Helpers;
using NoDb.Web.Middleware;
using NoDb.Web.Models;

namespace NoDb.Web.Extensions
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