using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public interface IReservationsService
    {
        Guid? Create(CreateReservation command);
        bool Delete(DeleteReservation command);
        ReservationDto Get(ReservationId id);
        IEnumerable<ReservationDto> GetAllWeekly();
        bool Update(ChangeReservationLicensePlate command);
    }
}
