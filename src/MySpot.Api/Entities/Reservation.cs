using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class Reservation
{
    public Guid Id { get; }
    public Guid ParkingSpotId { get; private set; }
    public string EmployeeName { get; private set; }
    public string LicensePLate { get; private set; }
    public DateTime Date { get; private set; }

    public Reservation(Guid id, string employeeName, string licensePLate, DateTime date, Guid parkingSpotName)
    {
        Id = id;
        EmployeeName = employeeName;
        ChangeLicensePlate(licensePLate);
        Date = date;
        ParkingSpotId = parkingSpotName;
    }

    public void ChangeLicensePlate(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
        {
            throw new EmptyLicensePlateEception();
        }

        LicensePLate = licensePlate;
    }
}
