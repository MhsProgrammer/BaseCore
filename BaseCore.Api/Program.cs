using BaseCore.Api;
using BaseCore.Application.Contracts.Identity;
using BaseCore.Identity.Repositories;
using BaseCore.Identity.Services;

var builder = WebApplication.CreateBuilder(args);





var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

//using var scope = app.Services.CreateScope();
//var black = scope.ServiceProvider.GetService<BlacklistTokenRepository>();

await app.SeedUser();


await app.RunAsync();

public partial class Program { }
