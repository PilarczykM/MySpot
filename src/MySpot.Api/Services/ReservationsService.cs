using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static List<WeeklyParkingSpot> _weeklyParkingSpot;

    public ReservationDto Get(ReservationId id) =>
        GetAllWeekly().SingleOrDefault(x => x.Id == id.Value);

    public ReservationsService(List<WeeklyParkingSpot> weeklyParkingSpots)
    {
        _weeklyParkingSpot = weeklyParkingSpots;
    }

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
