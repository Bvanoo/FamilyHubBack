using FamHubBack.Data;
using FamHubBack.Data.Entities;
using FamHubBack.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CalendarController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
    {
        if (eventDto == null) return BadRequest();

        var newEvent = new CalendarEvent
        {
            Title = eventDto.Title,
            Type = eventDto.Type,
            Color = eventDto.Color,
            Start = eventDto.Start,
            End = eventDto.End,
            UserId = eventDto.UserId
        };

        _context.CalendarEvents.Add(newEvent);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Événement enregistré !", id = newEvent.Id });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetUserEvents(int userId)
    {
        var events = await _context.CalendarEvents
            .Where(e => e.UserId == userId)
            .Select(e => new {
                id = e.Id,
                title = e.Title,
                start = e.Start,
                end = e.End,
                backgroundColor = e.Color,
                borderColor = e.Color,
                extendedProps = new
                {
                    type = e.Type
                }
            })
            .ToListAsync();

        return Ok(events);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto eventDto)
    {
        var existingEvent = await _context.CalendarEvents.FindAsync(id);
        if (existingEvent == null) return NotFound();

        existingEvent.Title = eventDto.Title;
        existingEvent.Start = eventDto.Start;
        existingEvent.End = eventDto.End;
        existingEvent.Color = eventDto.Color;
        existingEvent.Type = eventDto.Type;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventItem = await _context.CalendarEvents.FindAsync(id);
        if (eventItem == null) return NotFound();

        _context.CalendarEvents.Remove(eventItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}