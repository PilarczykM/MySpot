using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    public const int MaxCapacity = 2;
    public ParkingSpotId Id { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public Week Week { get; private set; }
    public Capacity Capacity { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private WeeklyParkingSpot(ParkingSpotId id, ParkingSpotName name, Week week, Capacity capacity)
    {
        Id = id;
        Name = name;
        Week = week;
        Capacity = capacity;
    }

    public static WeeklyParkingSpot Create(ParkingSpotId id, ParkingSpotName name, Week week)
        => new(id, name, week, MaxCapacity);

    internal void AddReservation(Reservation reservation, Date now)
    {
        var isInvalidDate =
            reservation.Date < Week.From || reservation.Date > Week.To || reservation.Date < now;

        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }

        var dateCapaciy = _reservations
            .Where(x => x.Date == reservation.Date)
            .Sum(x => x.Capacity);

        if (dateCapaciy + reservation.Capacity > Capacity)
        {
            throw new ParkingSpotCapacityExceededException(reservation.ParkingSpotId);
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

    public void RemoveReservations(IEnumerable<Reservation> reservations)
        => _reservations.RemoveWhere(x => reservations.Any(r => r.Id == x.Id));
}
