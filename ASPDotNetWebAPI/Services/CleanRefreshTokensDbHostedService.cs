using ASPDotNetWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace ASPDotNetWebAPI.Services
{
    public class CleanRefreshTokensDbHostedService : BackgroundService
    {
        private readonly ILogger<CleanRefreshTokensDbHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _timeout;

        public CleanRefreshTokensDbHostedService(ILogger<CleanRefreshTokensDbHostedService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _timeout = TimeSpan.FromHours(configuration.GetValue("CleanRefreshTokensDbHostedServiceSettings:TimeoutHouse", 1));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        _logger.LogInformation("cleanrefreshtokensdbhostedservice is working.");

                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        DateTime currentDateTime = DateTime.UtcNow;
                        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM public.\"RefreshTokens\" WHERE \"Expires\" < @currentDateTime",
                                  new NpgsqlParameter("@currentDateTime", NpgsqlDbType.TimestampTz) { Value = currentDateTime });

                        _logger.LogInformation("refreshtokens cleaning is finished!");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, "An error occurred in CleanRefreshTokensDbHostedService.");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, $"Operation DELETE FROM public.\"RefreshTokens\" WHERE \"Expires\" < '{DateTime.UtcNow}' canceled in CleanRefreshTokensDbHostedService.");
                }

                await Task.Delay(_timeout, stoppingToken);
            }
        }
    }
}
