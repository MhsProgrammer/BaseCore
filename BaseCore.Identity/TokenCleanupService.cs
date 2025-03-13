using BaseCore.Identity.IdentityContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace BaseCore.Identity
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);  // تنظیم برای اجرای هر ۱ ساعت

        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TokenCleanupService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<BaseCoreIdentityContext>();

                        _logger.LogInformation("Checking for expired tokens...");

                        int deletedTokens = await dbContext.BlackListTokens
                            .Where(t => t.ExpiryDate < DateTime.UtcNow)
                            .ExecuteDeleteAsync();

                        _logger.LogInformation($"Deleted {deletedTokens} expired tokens.");

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while cleaning up expired tokens.");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("TokenCleanupService is stopping.");
        }
    }
}
