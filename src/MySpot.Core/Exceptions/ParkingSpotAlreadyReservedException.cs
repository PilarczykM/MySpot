using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotAlreadyReservedException : CustomException
{
    public string Name { get; }
    public Date Date { get; }

    public ParkingSpotAlreadyReservedException(string name, Date date)
        : base($"Parking spot: {name} is already reserved at: {date:d}")
    {
        Name = name;
        Date = date;
    }
}
