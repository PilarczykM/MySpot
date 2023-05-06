using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    public Guid Id { get; }
    public string Name { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(Guid id, string name, DateTime from, DateTime to)
    {
        Id = id;
        Name = name;
        From = from;
        To = to;
    }

    public void AddReservation(Reservation reservation)
    {
        var isInvalidDate =
            reservation.Date.Date < From
            || reservation.Date.Date > To
            || reservation.Date.Date < DateTime.UtcNow.Date;

        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }

        var reservationAlreadyExists = Reservations.Any(r => r.Date.Date == reservation.Date.Date);

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
