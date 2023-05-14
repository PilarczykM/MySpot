using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationsService
{
    private static IWeeklyParkingSpotRepository _weeklyParkingSpotsRepository;
    private readonly IClock _clock;
    private readonly IParkingReservationService _parkingReservationService;

    public ReservationsService(
        IWeeklyParkingSpotRepository weeklyParkingSpotsRepository,
        IClock clock,
        IParkingReservationService parkingReservationService
    )
    {
        _weeklyParkingSpotsRepository = weeklyParkingSpotsRepository;
        _clock = clock;
        _parkingReservationService = parkingReservationService;
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
                        EmployeeName = x is VehicleReservation vr ? vr.EmployeeName.Value : string.Empty,
                        ParkingSpotId = x.ParkingSpotId.Value,
                        Date = x.Date.Value.Date
                    }
            );
    }

    public async Task<Guid?> ReserveForCleaningAsync(ReserveParkingSpotForCleaning command)
    {
        var week = new Week(command.Date);
        var weeklyParkingSpots = await _weeklyParkingSpotsRepository.GetByWeekAsync(week);

        _parkingReservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));


        var tasks = weeklyParkingSpots.Select(x => _weeklyParkingSpotsRepository.UpdateAsync(x));
        await Task.WhenAll(tasks);

        //foreach (var parkingSpot in weeklyParkingSpots)
        //{
        //    await _weeklyParkingSpotsRepository.UpdateAsync(parkingSpot);
        //}

        return null;
    }

    public async Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicle command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var week = new Week(_clock.Current());

        var weeklyParkingSpots = (
            await _weeklyParkingSpotsRepository.GetByWeekAsync(week)
        ).ToList();

        var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);
        if (parkingSpotToReserve is null)
        {
            return default;
        }

        var reservation = new VehicleReservation(
            command.ReservationId,
            new(command.Date),
            command.ParkingSpotId,
            command.EmployeeName,
            command.LicensePlate
        );

        _parkingReservationService.ReserveSpotForVehicle(
            weeklyParkingSpots,
            JobTitle.Employee,
            parkingSpotToReserve,
            reservation
        );

        await _weeklyParkingSpotsRepository.UpdateAsync(parkingSpotToReserve);

        return reservation.Id;
    }

    public async Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotReservationAsync(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            return false;
        }

        var existingReservation = weeklyParkingSpot.Reservations.OfType<VehicleReservation>().SingleOrDefault(
            x => x.Id.Value == command.ReservationId
        );

        if (existingReservation is null)
        {
            return false;
        }

        if (existingReservation.Date.Value.Date <= _clock.Current())
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

    private static async Task<WeeklyParkingSpot> GetWeeklyParkingSpotReservationAsync(
        ReservationId reservationId
    )
    {
        var result = await _weeklyParkingSpotsRepository.GetAllAsync();

        return result.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
    }

}
