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
                .Include(e => e.User)
                .Where(e =>
                    (e.UserId == currentUserId && !e.GroupId.HasValue) ||
                    (e.GroupId.HasValue && myGroupsIds.Contains(e.GroupId.Value))
                )
                .ToListAsync();

            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Start = e.Start,
                End = e.End,
                Title = e.Title,
                Description = e.Description,
                Type = e.Type ?? "Disponible",
                IsPrivate = e.IsPrivateEvent,
                GroupId = e.GroupId,
                UserId = e.UserId,
                Color = e.GroupId.HasValue ? "#3b82f6" : e.Color,
                UserName = e.User?.Name ?? "Utilisateur",
                UserPicture = e.User?.ProfilePictureUrl
            });

            return Ok(eventDtos);
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroupEvents(int groupId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int currentUserId))
            {
                currentUserId = 1;
            }

            var isMember = await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == currentUserId && gm.Status == MemberStatus.Accepted);

            if (!isMember) return Forbid();

            var groupMemberIds = await _context.GroupMembers
                .Where(gm => gm.GroupId == groupId && gm.Status == MemberStatus.Accepted)
                .Select(gm => gm.UserId)
                .ToListAsync();

            var events = await _context.CalendarEvents
                .Include(e => e.User)
                .Where(e =>
                    e.GroupId == groupId ||
                    (!e.GroupId.HasValue && groupMemberIds.Contains(e.UserId))
                )
                .ToListAsync();

            var eventDtos = events.Select(e =>
            {
                bool isHiddenFromMe = e.UserId != currentUserId && !e.GroupId.HasValue && e.IsPrivateEvent;

                return new EventDto
                {
                    Id = e.Id,
                    Start = e.Start,
                    End = e.End,
                    Title = isHiddenFromMe ? "Indisponible" : e.Title,
                    Description = isHiddenFromMe ? "" : e.Description,
                    Type = isHiddenFromMe ? "Indisponible" : (e.Type ?? "Disponible"),
                    IsPrivate = e.IsPrivateEvent,
                    GroupId = e.GroupId,
                    UserId = e.UserId,
                    Color = isHiddenFromMe ? "#9ca3af" : (e.GroupId.HasValue ? "#3b82f6" : e.Color),
                    UserName = e.User?.Name ?? "Utilisateur",
                    UserPicture = e.User?.ProfilePictureUrl
                };
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