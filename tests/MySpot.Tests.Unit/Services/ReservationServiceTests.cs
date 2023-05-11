using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange
    private readonly Clock _clock = new();
    private readonly ReservationsService _reservationsService;
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpot;

    public ReservationServiceTests()
    {
        _weeklyParkingSpot = new()
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
        };
        _reservationsService = new ReservationsService(_weeklyParkingSpot);
    }
    #endregion

    [Fact]
    public void Create_Reservation_For_Not_Taken_Date_Adds_Reservation()
    {
        //ARRANGE
        var parkingSpot = _weeklyParkingSpot.FirstOrDefault();
        var command = new CreateReservation(
            parkingSpot.Id,
            Guid.NewGuid(),
            DateTime.UtcNow.AddMinutes(5),
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
