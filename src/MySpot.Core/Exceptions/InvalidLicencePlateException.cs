namespace MySpot.Core.Exceptions;

public class InvalidLicencePlateException : CustomException
{
    public string LicenseName { get; }

    public InvalidLicencePlateException(string licenseName)
        : base($"License plate: {licenseName} is invalid.")
    {
        LicenseName = licenseName;
    }
}
