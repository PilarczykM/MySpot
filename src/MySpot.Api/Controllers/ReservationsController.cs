using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;

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
    public async Task<ActionResult<IEnumerable<Reservation>>> Get() =>
        Ok(await _reservationsService.GetAllWeeklyAsync());

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Reservation>> Get(Guid id)
    {
        var reservation = await _reservationsService.GetAsync(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost("vehicle")]
    public async Task<ActionResult<Reservation>> Post(ReserveParkingSpotForVehicle command)
    {
        var reservationId = await _reservationsService.ReserveForVehicleAsync(
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

    [HttpPost("cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaning command)
    {
        await _reservationsService.ReserveForCleaningAsync(command);

        return Ok();
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<Reservation>> Put(Guid id, ChangeReservationLicensePlate command)
    {
        var updated = await _reservationsService.ChangeReservationLicensePlateAsync(command with { ReservationId = id });

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
