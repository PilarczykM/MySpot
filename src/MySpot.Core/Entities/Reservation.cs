using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public abstract class Reservation
{
    public ReservationId Id { get; private set; }
    public ParkingSpotId ParkingSpotId { get; private set; }
    public Capacity Capacity { get; private set; }
    public Date Date { get; private set; }

    protected Reservation()
    {
    }

    public Reservation(ReservationId id, Date date, ParkingSpotId parkingSpotId, Capacity capacity)
    {
        Id = id;
        Date = date;
        ParkingSpotId = parkingSpotId;
        Capacity = capacity;
    }
}
