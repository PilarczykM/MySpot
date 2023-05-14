using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public sealed class VehicleReservation : Reservation
    {
        public EmployeeName EmployeeName { get; private set; }
        public LicensePlate LicensePLate { get; private set; }

        private VehicleReservation()
        {
        }

        public VehicleReservation(
            ReservationId id,
            Date date,
            ParkingSpotId parkingSpotId,
            EmployeeName employeeName,
            LicensePlate licensePLate
        )
            : base(id, date, parkingSpotId)
        {
            EmployeeName = employeeName;
            LicensePLate = licensePLate;
        }

        public void ChangeLicensePlate(string licensePlate) => LicensePLate = licensePlate;
    }
}
