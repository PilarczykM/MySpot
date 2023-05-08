namespace MySpot.Api.Exceptions;

public sealed class InvalidEntityIdException : CustomException
{
    public Guid Id { get; }

    public InvalidEntityIdException(Guid id)
        : base($"Cannot set: {id} as entity identifier.")
    {
        Id = id;
    }
}
