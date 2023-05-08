using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public sealed record LicensePlate
{
    public string Value { get; }

    public LicensePlate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyLicensePlateEception();
        }

        if (value.Length is < 5 or > 8)
        {
            throw new InvaliceLicensePlateException(value);
        }

        Value = value;
    }

    public static implicit operator LicensePlate(string licensePLate) => new(licensePLate);

    public static implicit operator string(LicensePlate licensePLate) => licensePLate.Value;
}
