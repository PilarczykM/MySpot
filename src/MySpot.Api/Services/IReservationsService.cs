using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services
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
