using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    public ParkingSpotId Id { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public Week Week { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(ParkingSpotId id, ParkingSpotName name, Week week)
    {
        Id = id;
        Name = name;
        Week = week;
    }

    internal void AddReservation(Reservation reservation, Date now)
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
