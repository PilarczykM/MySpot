using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static readonly Clock _clock = new();
    private static readonly List<WeeklyParkingSpot> _weeklyParkingSpot =
        new()
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
                new Week(_clock.Current())
            ),
        };

    public ReservationDto Get(ReservationId id) =>
        GetAllWeekly().SingleOrDefault(x => x.Id == id.Value);

    public IEnumerable<ReservationDto> GetAllWeekly() =>
        _weeklyParkingSpot
            .SelectMany(x => x.Reservations)
            .Select(
                x =>
                    new ReservationDto()
                    {
                        Id = x.Id.Value,
                        EmployeeName = x.EmployeeName.Value,
                        ParkingSpotId = x.ParkingSpotId.Value,
                        Date = x.Date.Value.Date
                    }
            );

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _weeklyParkingSpot.SingleOrDefault(
            x => x.Id.Value == command.ParkingSpotId
        );
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.EmployeeName,
            command.LicensePlate,
            new(command.Date),
            command.ParkingSpotId
        );

        weeklyParkingSpot.AddReservation(reservation, Date.Now);

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            x => x.Id.Value == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePLate);

        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            r => r.Id.Value == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(existingReservation);

        return true;
    }

    private static WeeklyParkingSpot GetWeeklyParkingSpotReservation(ReservationId reservationId) =>
        _weeklyParkingSpot.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}
