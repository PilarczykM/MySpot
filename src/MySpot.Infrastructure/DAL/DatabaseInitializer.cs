using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
            dbContext.Database.Migrate();

            var weeklyParkingSpot = dbContext.WeeklyParkingSpots.ToList();
            if (weeklyParkingSpot.Any())
            {
                return Task.CompletedTask;
            }
            var clock = new Clock();
            weeklyParkingSpot = new List<WeeklyParkingSpot>()
            {
                WeeklyParkingSpot.Create(
                    Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    "P1",
                    new Week(clock.Current())
                ),
                WeeklyParkingSpot.Create(
                    Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    "P2",
                    new Week(clock.Current())
                ),
                WeeklyParkingSpot.Create(
                    Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    "P3",
                    new Week(clock.Current())
                ),
                WeeklyParkingSpot.Create(
                    Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    "P4",
                    new Week(clock.Current())
                ),
                WeeklyParkingSpot.Create(
                    Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    "P5",
                    new Week(clock.Current())
                ),
            };

            dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpot);
            dbContext.SaveChanges();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
