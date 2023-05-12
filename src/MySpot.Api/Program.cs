using MySpot.Application;
using MySpot.Infrastructure;
using MySpot.Core;
using MySpot.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCore().AddApplication().AddInfrastructure().AddControllers();

var app = builder.Build();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
    dbContext.Database.Migrate();

    var weeklyParkingSpot = dbContext.WeeklyParkingSpots.ToList();
    if (!weeklyParkingSpot.Any())
    {
        var clock = new Clock();
        weeklyParkingSpot = new List<WeeklyParkingSpot>()
        {
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "P1",
                new Week(clock.Current())
            ),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                "P2",
                new Week(clock.Current())
            ),
        };

        dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpot);
        dbContext.SaveChanges();
    }
}

app.Run();
