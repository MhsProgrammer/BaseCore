using Microsoft.AspNetCore.Diagnostics;

namespace BaseCore.Api.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
        public static IApplicationBuilder UseBlackListToklen(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlackListTokenMiddleware>();
        }
    }
}
