using Microsoft.AspNetCore.Mvc;
using MySpot.Core.Entities;
using MySpot.Application.Services;
using MySpot.Application.Commands;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IClock _clock;
    private readonly IReservationsService _reservationsService;

    public ReservationsController(IClock clock, IReservationsService reservationsService)
    {
        _clock = clock;
        _reservationsService = reservationsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservationsService.GetAllWeekly());

    [HttpGet("{id:Guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _reservationsService.Get(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post(CreateReservation command)
    {
        var reservationId = _reservationsService.Create(
            command with
            {
                ReservationId = Guid.NewGuid()
            }
        );
        if (reservationId is null)
        {
            return BadRequest("Unknown parking spot");
        }

        return CreatedAtAction(nameof(Get), new { id = reservationId }, null);
    }

    [HttpPut("{id:Guid}")]
    public ActionResult<Reservation> Put(Guid id, ChangeReservationLicensePlate command)
    {
        var updated = _reservationsService.Update(command with { ReservationId = id });

        if (!updated)
        {
            return NotFound("Reservation not found");
        }

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public ActionResult Delete(Guid id)
    {
        var deleted = _reservationsService.Delete(new(id));

        if (!deleted)
        {
            return NotFound("Reservation not found.");
        }

        return NoContent();
    }
}
