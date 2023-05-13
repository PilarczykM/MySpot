using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
    {
        private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

        private readonly IClock _clock;

        public InMemoryWeeklyParkingSpotRepository(IClock clock)
        {
            _clock = clock;
            _weeklyParkingSpots = new List<WeeklyParkingSpot>()
            {
                new(
                    Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    "P1",
                    new Week(_clock.Current())
                ),
                new(
                    Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    "P2",
                    new Week(_clock.Current())
                ),
                new(
                    Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    "P3",
                    new Week(_clock.Current())
                ),
                new(
                    Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    "P4",
                    new Week(_clock.Current())
                ),
                new(
                    Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    "P5",
                    new Week(clock.Current())
                )
            };
        }

        public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
        {
            _weeklyParkingSpots.Add(weeklyParkingSpot);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot)
        {
            _weeklyParkingSpots.Remove(weeklyParkingSpot);
            return Task.CompletedTask;
        }

        public Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id) =>
            Task.FromResult(_weeklyParkingSpots.SingleOrDefault(x => x.Id == id));

        public Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync() => Task.FromResult(_weeklyParkingSpots.AsEnumerable());

        public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot) => Task.CompletedTask;
    }
}
