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

    public async Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        await _weeklyParkingSpots.AddAsync(weeklyParkingSpot);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Remove(weeklyParkingSpot);
        await _dbContext.SaveChangesAsync();
    }

    public Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id) =>
        _weeklyParkingSpots.Include(x => x.Reservations).SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        var result = await _weeklyParkingSpots.Include(x => x.Reservations).ToListAsync();
        return result.AsEnumerable();
    }

    public async Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Update(weeklyParkingSpot);
        await _dbContext.SaveChangesAsync();
    }
}
