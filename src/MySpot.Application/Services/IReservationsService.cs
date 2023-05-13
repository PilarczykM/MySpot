using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public interface IReservationsService
    {
        Task<Guid?> CreateAsync(CreateReservation command);
        Task<bool> DeleteAsync(DeleteReservation command);
        Task<ReservationDto> GetAsync(ReservationId id);
        Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
        Task<bool> UpdateAsync(ChangeReservationLicensePlate command);
    }
}
