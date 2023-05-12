using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class Reservation
{
    public ReservationId Id { get; private set; }
    public ParkingSpotId ParkingSpotId { get; private set; }
    public EmployeeName EmployeeName { get; private set; }
    public LicensePlate LicensePLate { get; private set; }
    public Date Date { get; private set; }

    public Reservation(
        ReservationId id,
        EmployeeName employeeName,
        LicensePlate licensePLate,
        Date date,
        ParkingSpotId parkingSpotId
    )
    {
        Id = id;
        EmployeeName = employeeName;
        LicensePLate = licensePLate;
        Date = date;
        ParkingSpotId = parkingSpotId;
    }

    public void ChangeLicensePlate(string licensePlate) => LicensePLate = licensePlate;
}
