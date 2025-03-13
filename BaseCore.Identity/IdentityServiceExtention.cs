using BaseCore.Application.Contracts.Identity;
using BaseCore.Application.Models.Authentication;
using BaseCore.Identity.IdentityContext;
using BaseCore.Identity.Models;
using BaseCore.Identity.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BaseCore.Identity
{
    public static class IdentityServiceExtention
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<AuthCookie>(configuration.GetSection("AuthCookie"));

            services.AddDbContext<BaseCoreIdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("BaseCoreIdentityConnection"),
                b => b.MigrationsAssembly(typeof(BaseCoreIdentityContext).Assembly.FullName)));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<BaseCoreIdentityContext>().AddDefaultTokenProviders();
            //services.AddScoped<IBlacklistTokenRepository, BlacklistTokenRepository>();

            //services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {

                            context.Token = context.HttpContext.Request.Cookies["AuthToken"];
                            return Task.CompletedTask;

                        }
                    };

                    //o.Events = new JwtBearerEvents()
                    //{
                    //    OnAuthenticationFailed = c =>
                    //    {
                    //        c.NoResult();
                    //        c.Response.StatusCode = 500;
                    //        c.Response.ContentType = "text/plain";
                    //        return c.Response.WriteAsync(c.Exception.ToString());
                    //    },
                    //    OnChallenge = context =>
                    //    {
                    //        context.HandleResponse();
                    //        context.Response.StatusCode = 401;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonSerializer.Serialize("401 Not authorized");
                    //        return context.Response.WriteAsync(result);
                    //    },
                    //    OnForbidden = context =>
                    //    {
                    //        context.Response.StatusCode = 403;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonSerializer.Serialize("403 Not authorized");
                    //        return context.Response.WriteAsync(result);
                    //    }
                    //};
                });
        }
    }
}
