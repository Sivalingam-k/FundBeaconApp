using FundBeacon.Data;
using FundBeacon.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace FundBeacon.Application.Services
{
    public class UserCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<FundBeaconDbContext>();

                        var threshold = DateTime.UtcNow.AddHours(-3);

                        var usersToSoftDelete = await dbContext.Set<ApplicationUser>()
                                                    .Where(u => u.LastLogin < threshold && !u.IsDeleted)
                            .ToListAsync(stoppingToken);

                        foreach (var user in usersToSoftDelete)
                        {
                            user.IsDeleted = true;
                        }

                        if (usersToSoftDelete.Any())
                        {
                            await dbContext.SaveChangesAsync(stoppingToken);
                            Console.WriteLine($"{usersToSoftDelete.Count} users marked as deleted.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UserCleanupService] Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run every 10 minutes
            }
        }
    }
}
