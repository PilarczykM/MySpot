using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Core.Repositories;
using MySpot.Application.Services;

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

        public void Add(WeeklyParkingSpot weeklyParkingSpot) =>
            _weeklyParkingSpots.Add(weeklyParkingSpot);

        public void Delete(WeeklyParkingSpot weeklyParkingSpot) =>
            _weeklyParkingSpots.Remove(weeklyParkingSpot);

        public WeeklyParkingSpot Get(ParkingSpotId id) =>
            _weeklyParkingSpots.SingleOrDefault(x => x.Id == id);

        public IEnumerable<WeeklyParkingSpot> GetAll() => _weeklyParkingSpots;

        public void Update(WeeklyParkingSpot weeklyParkingSpot) { }
    }
}
