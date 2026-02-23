using FamHubBack.Data;
using FamHubBack.Data.Entities;
using FamHubBack.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FamHubBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            if (eventDto == null) return BadRequest("Données vides");
            var userExists = await _context.Users.AnyAsync(u => u.Id == eventDto.UserId);
            if (!userExists)
            {
                return BadRequest($"L'utilisateur ID {eventDto.UserId} n'existe pas en base.");
            }

            var newEvent = new CalendarEvent
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                Type = eventDto.Type,
                Color = eventDto.Color,
                Start = eventDto.Start,
                End = eventDto.End,
                UserId = eventDto.UserId,
                GroupId = eventDto.GroupId,
                IsPrivateEvent = eventDto.IsPrivate,
                MaskDetails = eventDto.MaskDetails
            };

            try
            {
                _context.CalendarEvents.Add(newEvent);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Événement enregistré !", id = newEvent.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur SQL", error = ex.Message, inner = ex.InnerException?.Message });
            }
        }
        [HttpGet("unified")]
        public async Task<IActionResult> GetUnifiedCalendar()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int currentUserId))
            {
                currentUserId = 1;
            }

            var myGroupsIds = await _context.GroupMembers
                .Where(gm => gm.UserId == currentUserId && gm.Status == MemberStatus.Accepted)
                .Select(gm => gm.GroupId)
                .ToListAsync();

            var events = await _context.CalendarEvents
                .Include(e => e.Group)
                .Where(e =>
                    e.UserId == currentUserId ||
                    (e.GroupId.HasValue && myGroupsIds.Contains(e.GroupId.Value))
                )
                .ToListAsync();

            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Start = e.Start,
                End = e.End,
                Title = (e.UserId != currentUserId && e.MaskDetails) ? "Indisponible" : e.Title,
                Description = (e.UserId != currentUserId && e.MaskDetails) ? "" : e.Description,
                IsPrivate = e.IsPrivateEvent,
                GroupId = e.GroupId,
                UserId = e.UserId,
                Color = e.GroupId.HasValue ? "#3788d8" : e.Color,
                Type = e.Type
            });

            return Ok(eventDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto eventDto)
        {
            var existingEvent = await _context.CalendarEvents.FindAsync(id);
            if (existingEvent == null) return NotFound();

            existingEvent.Title = eventDto.Title;
            existingEvent.Description = eventDto.Description;
            existingEvent.Start = eventDto.Start;
            existingEvent.End = eventDto.End;
            existingEvent.Color = eventDto.Color;
            existingEvent.Type = eventDto.Type;
            existingEvent.IsPrivateEvent = eventDto.IsPrivate;
            existingEvent.MaskDetails = eventDto.MaskDetails;

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
}