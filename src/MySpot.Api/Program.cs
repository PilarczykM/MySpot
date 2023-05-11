using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddSingleton<IClock, Clock>()
    .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
    .AddSingleton<IReservationsService, ReservationsService>()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
