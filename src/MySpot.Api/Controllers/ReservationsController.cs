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
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservationsService.GetAllWeeklyAsync());

    [HttpGet("{id:Guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _reservationsService.GetAsync(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post(CreateReservation command)
    {
        var reservationId = _reservationsService.CreateAsync(
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
    public async Task<ActionResult<Reservation>> Put(Guid id, ChangeReservationLicensePlate command)
    {
        var updated = await _reservationsService.UpdateAsync(command with { ReservationId = id });

        if (!updated)
        {
            return NotFound("Reservation not found");
        }

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _reservationsService.DeleteAsync(new(id));

        if (!deleted)
        {
            return NotFound("Reservation not found.");
        }

        return NoContent();
    }
}
