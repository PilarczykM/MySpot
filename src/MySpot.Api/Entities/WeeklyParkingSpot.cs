using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    public ParkingSpotId Id { get; }
    public ParkingSpotName Name { get; }
    public Week Week { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(Guid id, string name, Week week)
    {
        Id = id;
        Name = name;
        Week = week;
    }

    public void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate =
            reservation.Date < Week.From || reservation.Date > Week.To || reservation.Date < now;

        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }

        var reservationAlreadyExists = _reservations.Any(r => r.Date == reservation.Date);

        if (reservationAlreadyExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Reservation reservation)
    {
        var reservationExists = Reservations.Any(r => r.Id == reservation.Id);

        if (!reservationExists)
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);
        }

        _reservations.Remove(reservation);
    }
}
