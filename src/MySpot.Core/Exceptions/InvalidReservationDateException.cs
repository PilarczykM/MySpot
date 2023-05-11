using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class InvalidReservationDateException : CustomException
{
    public Date Date { get; }

    public InvalidReservationDateException(Date date)
        : base($"Reservation date {date:d} is invalid.")
    {
        Date = date;
    }
}
