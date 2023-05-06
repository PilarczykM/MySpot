using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
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
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_service.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post([FromBody] Reservation reservation)
    {
        var reservationId = _service.Create(reservation);
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

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Put([FromRoute] int id, [FromBody] Reservation reservation)
    {
        var updated = _service.Update(id, reservation);

        if (!updated)
        {
            return NotFound("Reservation not found");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var deleted = _service.Delete(id);

        if (!deleted)
        {
            return NotFound("Reservation not found.");
        }

        return NoContent();
    }
}
