using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpot =
        new()
        {
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "P1",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7)
            ),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                "P2",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7)
            ),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000003"),
                "P3",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7)
            ),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000004"),
                "P4",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7)
            ),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000005"),
                "P5",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(7)
            ),
        };

    public ReservationDto Get(Guid id) => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() =>
        _weeklyParkingSpot
            .SelectMany(x => x.Reservations)
            .Select(
                x =>
                    new ReservationDto()
                    {
                        Id = x.Id,
                        EmployeeName = x.EmployeeName,
                        ParkingSpotId = x.ParkingSpotId,
                        Date = x.Date
                    }
            );

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _weeklyParkingSpot.SingleOrDefault(
            x => x.Id == command.ParkingSpotId
        );
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.EmployeeName,
            command.LicensePlate,
            command.Date,
            command.ParkingSpotId
        );

        weeklyParkingSpot.AddReservation(reservation);

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
            x => x.Id == command.ReservationId
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
            r => r.Id == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(existingReservation);

        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotReservation(Guid reservationId) =>
        _weeklyParkingSpot.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}
