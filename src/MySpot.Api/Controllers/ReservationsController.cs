using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationsService _service;

    public ReservationsController()
    {
        _service = new ReservationsService();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_service.GetAllWeekly());

    [HttpGet("{id:Guid}")]
    public ActionResult<Reservation> Get(Guid id)
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post(CreateReservation command)
    {
        var reservationId = _service.Create(command with { ReservationId = Guid.NewGuid() });
        if (reservationId is null)
        {
            return BadRequest("Unknown parking spot");
        }

        if (reservationId is null)
        {
            return BadRequest("Parking spot already in use");
        }

        return CreatedAtAction(nameof(Get), new { id = reservationId }, null);
    }

    [HttpPut("{id:Guid}")]
    public ActionResult<Reservation> Put(Guid id, ChangeReservationLicensePlate command)
    {
        var updated = _service.Update(command with { ReservationId = id });

        if (!updated)
        {
            return NotFound("Reservation not found");
        }

        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public ActionResult Delete(Guid id)
    {
        var deleted = _service.Delete(new(id));

        if (!deleted)
        {
            return NotFound("Reservation not found.");
        }

        return NoContent();
    }
}
