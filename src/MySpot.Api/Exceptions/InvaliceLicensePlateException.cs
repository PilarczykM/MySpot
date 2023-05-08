namespace MySpot.Api.Exceptions;

public class InvaliceLicensePlateException : CustomException
{
    public string LicenseName { get; }

    public InvaliceLicensePlateException(string licenseName)
        : base($"License plate: {licenseName} is invalid.")
    {
        LicenseName = licenseName;
    }
}
