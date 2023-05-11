using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Tests.Unit.Shared;
using MySpot.Infrastructure.Repositories;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange
    private readonly IClock _clock;
    private readonly IReservationsService _reservationsService;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationsService = new ReservationsService(_weeklyParkingSpotRepository, _clock);
    }
    #endregion

    [Fact]
    public void Create_Reservation_For_Not_Taken_Date_Adds_Reservation()
    {
        //ARRANGE
        var parkingSpot = _weeklyParkingSpotRepository.GetAll().FirstOrDefault();
        var command = new CreateReservation(
            parkingSpot.Id,
            Guid.NewGuid(),
            _clock.Current().AddMinutes(5),
            "Mark",
            "XYZ123"
        );

        //ACT
        var reservationId = _reservationsService.Create(command);

        //ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
}
