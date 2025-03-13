using BaseCore.Identity;
using BaseCore.Application;
using BaseCore.Persistance;
using BaseCore.Infrastructure;
using BaseCore.Persistance.Context;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BaseCore.Api.Middlewares;
using BaseCore.Identity.IdentityContext;
using BaseCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using BaseCore.Identity.Repositories;
using BaseCore.Application.Contracts.Identity;
using BaseCore.Identity.Services;
using BaseCore.Application.Contracts;
using BaseCore.Api.Services;
using BaseCore.Application.Models.Authentication;

namespace BaseCore.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
        this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);


            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddHostedService<TokenCleanupService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IBlacklistTokenRepository, BlacklistTokenRepository>();
            builder.Services.Configure<AuthCookie>(builder.Configuration.GetSection("AuthCookie"));




            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();



            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder
                .AllowCredentials()
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            

            return builder.Build();

        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BaseCore");
                });
            }

            app.UseHttpsRedirection();

            //app.UseRouting();
            app.UseBlackListToklen();

            app.UseAuthentication();

            app.UseCustomExceptionHandler();

            app.UseCors("Open");

            app.UseAuthorization();

            app.MapControllers();

            


            return app;

        }
        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BaseCore API",

                });

                //c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }

        public static async Task SeedUser(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            try
            {
                var context = scope.ServiceProvider.GetService<BaseCoreIdentityContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();

                var user = new AppUser()
                {
                    UserName = "Hamed",
                };
                var user2 = new AppUser()
                {
                    UserName = "User",
                };
                var role = new AppRole()
                {
                    Name = "SuperAdmin"
                };

                var role2 = new AppRole()
                {
                    Name = "User"
                };

                var isUserExist = await userManager.FindByNameAsync(user.UserName);
                var isUser2Exist = await userManager.FindByNameAsync(user2.UserName);
                var isRoleExist = await roleManager.FindByNameAsync(role.Name);
                var isRole2Exist = await roleManager.FindByNameAsync(role2.Name);
                if (isUserExist == null)
                {
                    try
                    {
                        var result = await userManager.CreateAsync(user, "@#$Hamed.Cr7");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }
                }

                if (isUser2Exist == null)
                {
                    try
                    {
                        var result = await userManager.CreateAsync(user2, "@#$Hamed.Cr7");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }
                }
                if (isRoleExist == null)
                {
                    try
                    {
                        var result1 = await roleManager.CreateAsync(role);
                        if (result1.Succeeded)
                        {
                            try
                            {
                                var result2 = await userManager.AddToRoleAsync(user, "SuperAdmin");

                            }
                            catch (Exception ex)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }

                }
                if (isRole2Exist == null)
                {
                    try
                    {
                        var result1 = await roleManager.CreateAsync(role2);
                        if (result1.Succeeded)
                        {
                            try
                            {
                                var result2 = await userManager.AddToRoleAsync(user2, "User");

                            }
                            catch (Exception ex)
                            {
                                throw new Exception();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception();
                    }

                }

                if (context != null)
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                //var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                //logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<BaseCoreContext>();
                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
