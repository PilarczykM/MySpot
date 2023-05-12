using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal sealed class PostgresWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly MySpotDbContext _dbContext;
    private readonly DbSet<WeeklyParkingSpot> _weeklyParkingSpots;

    public PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext)
    {
        _dbContext = dbContext;
        _weeklyParkingSpots = dbContext.WeeklyParkingSpots;
    }

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }

    public void Delete(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Remove(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }

    public WeeklyParkingSpot Get(ParkingSpotId id) =>
        _weeklyParkingSpots.Include(x => x.Reservations).SingleOrDefault(x => x.Id == id);

    public IEnumerable<WeeklyParkingSpot> GetAll() =>
        _weeklyParkingSpots.Include(x => x.Reservations).ToList();

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Update(weeklyParkingSpot);
        _dbContext.SaveChanges();
    }
}
