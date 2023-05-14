using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    #region ARRANGE
    private readonly DateTime _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new DateTime(2023, 01, 01);
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), "XYZ", new Week(_now));
    }
    #endregion

    [Theory]
    [InlineData(-1)]
    [InlineData(7)]
    public void AddReservation_Throws_InvalidReservationDateException_When_Date_Is_Invalid(
        int offsetDay
    )
    {
        //ARRANGE
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            new Date(_now.AddDays(offsetDay)),
            _weeklyParkingSpot.Id,
            "John Doe",
            "123456"
        );

        //ACT
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, new Date(_now))
        );

        //ASSERT
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void AddReservation_Throws_ParkingSpotAlreadyReservedException_When_Same_Reservation_Exists()
    {
        //ARRANGE
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            new Date(_now),
            _weeklyParkingSpot.Id,
            "John Doe",
            "123456"
        );
        _weeklyParkingSpot.AddReservation(reservation, new Date(_now));

        //ACT
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, new Date(_now))
        );

        //ASSERT
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    [Fact]
    public void AddReservation_Adds_Parking_Spot_Reservation_When_Spot_Not_Taken()
    {
        //ARRANGE
        const int expectedReservationCount = 1;
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            new Date(_now),
            _weeklyParkingSpot.Id,
            "John Doe",
            "123456"
        );

        //ACT
        var exception = Record.Exception(
            () => _weeklyParkingSpot.AddReservation(reservation, new Date(_now))
        );

        //ASSERT
        exception.ShouldBeNull();
        _weeklyParkingSpot.Reservations.Count().ShouldBe(expectedReservationCount);
    }
}
