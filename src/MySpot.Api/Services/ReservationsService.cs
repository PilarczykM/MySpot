using MySpot.Api.Models;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static int _id = 1;
    private static readonly List<Reservation> _reservations = new();
    private readonly List<string> _parkingSpotNames = new() { "P1", "P2", "P3", "P4", "P5" };

    public Reservation Get(int id) => _reservations.SingleOrDefault(r => r.Id == id);

    public IEnumerable<Reservation> GetAll() => _reservations;

    public int? Create(Reservation reservation)
    {
        if (_parkingSpotNames.All(spot => spot != reservation.ParkingSpotName))
        {
            return default;
        }

        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        var reservationAlreadyExists = _reservations.Any(
            r =>
                r.ParkingSpotName == reservation.ParkingSpotName
                && r.Date.Date == reservation.Date.Date
        );

        if (reservationAlreadyExists)
        {
            return default;
        }

        reservation.Id = _id;
        _id++;

        _reservations.Add(reservation);

        return reservation.Id;
    }

    public bool Update(int id, Reservation reservation)
    {
        var existingReservation = Get(id);

        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.LicensePLate = reservation.LicensePLate;

        return true;
    }

    public bool Delete(int id)
    {
        var existingReservation = _reservations.SingleOrDefault(r => r.Id == id);

        if (existingReservation is null)
        {
            return false;
        }

        _reservations.Remove(existingReservation);
        return true;
    }
}
