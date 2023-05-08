using MySpot.Api.ValueObjects;

namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationDateException : CustomException
{
    public Date Date { get; }

    public InvalidReservationDateException(Date date)
        : base($"Reservation date {date:d} is invalid.")
    {
        Date = date;
    }
}
