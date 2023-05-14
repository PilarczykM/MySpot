using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Policies;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange
    private readonly IClock _clock;
    private readonly IReservationsService _reservationsService;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _parkingReservationService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _parkingReservationService = new ParkingReservationService(new List<IReservationPolicy>()
        {
            new ManagerReservationPolicy(),
            new BossReservationPolicy(),
            new RegularEmployeeReservationPolicy(_clock)
        }, _clock);
        _reservationsService = new ReservationsService(_weeklyParkingSpotRepository, _clock, _parkingReservationService);
    }
    #endregion

    [Fact]
    public async Task Create_Reservation_For_Not_Taken_Date_Adds_Reservation()
    {
        //ARRANGE
        var parkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
        var parkingSpot = parkingSpots.FirstOrDefault();
        var command = new ReserveParkingSpotForVehicle(
            parkingSpot.Id,
            Guid.NewGuid(),
            _clock.Current().AddMinutes(5),
            "Mark",
            "XYZ123",
            2
        );

        //ACT
        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

        //ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
}
