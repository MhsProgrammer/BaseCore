using BaseCore.Application.Contracts.Identity;
using BaseCore.Identity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseCore.Api.Middlewares
{
    public class BlackListTokenMiddleware
    {
        private readonly IBlacklistTokenRepository _tokenRepository;
        private readonly RequestDelegate _next;

        public BlackListTokenMiddleware(RequestDelegate next, IBlacklistTokenRepository tokenRepository)
        {
            _next = next;
            _tokenRepository = tokenRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                bool isBlacklisted = await _tokenRepository
                    .IsTokenBlacklistedAsync(token);

                if (isBlacklisted)
                {
                    //_logger.LogWarning("Attempt to use a blacklisted token.");
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized: Token is blacklisted.");
                    return;
                }
            }

            await _next(context);
        }

        
    }
}
