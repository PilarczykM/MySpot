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

    public ReservationsService(
        IWeeklyParkingSpotRepository weeklyParkingSpotsRepository,
        IClock clock
    )
    {
        _weeklyParkingSpotsRepository = weeklyParkingSpotsRepository;
        _clock = clock;
    }

    public async Task<ReservationDto> GetAsync(ReservationId id)
    {
        var reservations = await GetAllWeeklyAsync();

        return reservations.SingleOrDefault(x => x.Id == id.Value);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
    {

        var weeklyParkingSpots = await _weeklyParkingSpotsRepository.GetAllAsync();

        return weeklyParkingSpots
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
    }

    public async Task<Guid?> CreateAsync(CreateReservation command)
    {
        var weeklyParkingSpot = await _weeklyParkingSpotsRepository.GetAsync(command.ParkingSpotId);
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
        await _weeklyParkingSpotsRepository.UpdateAsync(weeklyParkingSpot);

        return reservation.Id;
    }

    public async Task<bool> UpdateAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotReservationAsync(command.ReservationId);

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
        await _weeklyParkingSpotsRepository.UpdateAsync(weeklyParkingSpot);

        return true;
    }

    public async Task<bool> DeleteAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotReservationAsync(command.ReservationId);

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
        await _weeklyParkingSpotsRepository.DeleteAsync(weeklyParkingSpot);

        return true;
    }

    private static async Task<WeeklyParkingSpot> GetWeeklyParkingSpotReservationAsync(ReservationId reservationId)
    {
        var result = await _weeklyParkingSpotsRepository.GetAllAsync();

        return result
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
    }
}
