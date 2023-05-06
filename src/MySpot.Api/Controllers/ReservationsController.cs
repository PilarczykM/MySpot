using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private static int _id = 1;
    private static readonly List<Reservation> _reservations = new();
    private readonly List<string> _parkingSpotNames = new() { "P1", "P2", "P3", "P4", "P5" };

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservations);

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _reservations.SingleOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound("Reservation not found.");
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post([FromBody] Reservation reservation)
    {
        if (_parkingSpotNames.All(spot => spot != reservation.ParkingSpotName))
        {
            return BadRequest("Unknown parking spot");
        }

        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        var reservationAlreadyExists = _reservations.Any(
            r =>
                r.ParkingSpotName == reservation.ParkingSpotName
                && r.Date.Date == reservation.Date.Date
        );

        if (reservationAlreadyExists)
        {
            return BadRequest("Parking spot already in use");
        }

        reservation.Id = _id;
        _id++;

        _reservations.Add(reservation);

        return CreatedAtAction(nameof(Get), new { id = reservation.Id }, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Put([FromRoute] int id, [FromBody] Reservation reservation)
    {
        var existingReservation = _reservations.SingleOrDefault(r => r.Id == id);

        if (existingReservation is null)
        {
            return NotFound("Reservation not found.");
        }

        existingReservation.LicensePLate = reservation.LicensePLate;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var existingReservation = _reservations.SingleOrDefault(r => r.Id == id);

        if (existingReservation is null)
        {
            return NotFound("Reservation not found.");
        }

        _reservations.Remove(existingReservation);
        return NoContent();
    }
}
