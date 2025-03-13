using Azure;
using BaseCore.Application.Contracts.Identity;
using BaseCore.Application.Models.Authentication;
using BaseCore.Identity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BaseCore.Api.Middlewares
{
    public class BlackListTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AuthCookie _authCookie;

        public BlackListTokenMiddleware(RequestDelegate next, 
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AuthCookie> authCookie)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _authCookie = authCookie.Value;   
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Cookies["AuthToken"];


            if (!string.IsNullOrEmpty(token))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var blacklistTokenRepository = scope.ServiceProvider.GetRequiredService<IBlacklistTokenRepository>();

                    bool isBlacklisted = await blacklistTokenRepository
               .IsTokenBlacklistedAsync(token);

                    if (isBlacklisted)
                    {
                        context.Response.Cookies.Delete("AuthToken", new CookieOptions()
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.UtcNow.AddMinutes(_authCookie.DurationInMinutes)
                        });
                        //_logger.LogWarning("Attempt to use a blacklisted token.");
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("توکن نامعتبر است");
                        return;
                    }
                }


            }

            await _next(context);
        }


    }
}
