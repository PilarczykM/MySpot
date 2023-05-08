using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public sealed record ParkingSpotId
{
    public Guid Value { get; }

    public ParkingSpotId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException(value);
        }
        Value = value;
    }

    public static ReservationId Create() => new(Guid.NewGuid());

    public static implicit operator Guid(ParkingSpotId id) => id.Value;

    public static implicit operator ParkingSpotId(Guid id) => new(id);
}
