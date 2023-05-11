using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationsService
{
    private static IWeeklyParkingSpotRepository _weeklyParkingSpotsRepository;
    private readonly IClock _clock;

    public ReservationDto Get(ReservationId id) =>
        GetAllWeekly().SingleOrDefault(x => x.Id == id.Value);

    public ReservationsService(
        IWeeklyParkingSpotRepository weeklyParkingSpotsRepository,
        IClock clock
    )
    {
        _weeklyParkingSpotsRepository = weeklyParkingSpotsRepository;
        _clock = clock;
    }

    public IEnumerable<ReservationDto> GetAllWeekly() =>
        _weeklyParkingSpotsRepository
            .GetAll()
            .SelectMany(x => x.Reservations)
            .Select(
                x =>
                    new ReservationDto()
                    {
                        Id = x.Id.Value,
                        EmployeeName = x.EmployeeName.Value,
                        ParkingSpotId = x.ParkingSpotId.Value,
                        Date = x.Date.Value.Date
                    }
            );

    public Guid? Create(CreateReservation command)
    {
        var weeklyParkingSpot = _weeklyParkingSpotsRepository.Get(command.ParkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            command.ReservationId,
            command.EmployeeName,
            command.LicensePlate,
            new(command.Date),
            command.ParkingSpotId
        );

        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            x => x.Id.Value == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePLate);

        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(
            r => r.Id.Value == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(existingReservation);

        return true;
    }

    private static WeeklyParkingSpot GetWeeklyParkingSpotReservation(ReservationId reservationId) =>
        _weeklyParkingSpotsRepository
            .GetAll()
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}
