namespace MySpot.Api.Exceptions;

public sealed class ParkingSpotAlreadyReservedException : CustomException
{
    public ParkingSpotAlreadyReservedException(string name, DateTime date)
        : base($"Parking spot: {name} is already reserved at: {date:d}")
    {
        Name = name;
        Date = date;
    }

    public string Name { get; }
    public DateTime Date { get; }
}
