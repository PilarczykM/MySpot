using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

internal sealed class ParkingReservationService : IParkingReservationService
{
    private readonly IEnumerable<IReservationPolicy> _policies;
    private readonly IClock _clock;

    public ParkingReservationService(
        IEnumerable<IReservationPolicy> reservationPolicies,
        IClock clock
    )
    {
        _policies = reservationPolicies;
        _clock = clock;
    }

    public void ReserveParkingForCleaning(
        IEnumerable<WeeklyParkingSpot> weeklyParkingSpots,
        Date date
    )
    {
        foreach (var parkingSpot in weeklyParkingSpots)
        {
            var reservationsForSameDate = parkingSpot.Reservations.Where(x => x.Date == date);
            parkingSpot.RemoveReservations(reservationsForSameDate);

            var cleaningReservation = new CleaningReservation(
                ReservationId.Create(),
                date,
                parkingSpot.Id
            );
            parkingSpot.AddReservation(cleaningReservation, new Date(_clock.Current()));
        }
    }

    public void ReserveSpotForVehicle(
        IEnumerable<WeeklyParkingSpot> weeklyParkingSpots,
        JobTitle jobTitle,
        WeeklyParkingSpot parkingSpotToReserve,
        VehicleReservation reservation
    )
    {
        var parkingSpotId = parkingSpotToReserve.Id;
        var policy = _policies.SingleOrDefault(p => p.CanBeApplied(jobTitle));

        if (policy is null)
        {
            throw new NoReservationPolicyFoundException(jobTitle);
        }

        if (!policy.CanReserve(weeklyParkingSpots, reservation.EmployeeName))
        {
            throw new CannotReserveParkingSpotException(parkingSpotId);
        }

        parkingSpotToReserve.AddReservation(reservation, new Date(_clock.Current()));
    }
}
