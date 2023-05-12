namespace MySpot.Core.Exceptions;

public sealed class EmptyLicensePlateEception : CustomException
{
    public EmptyLicensePlateEception()
        : base("License plate can not be empty.") { }
}