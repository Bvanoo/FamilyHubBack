using FamHubBack.Data;
using FamHubBack.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FamHubBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
        {
            try
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return Unauthorized(new { message = "Impossible de lire l'ID utilisateur dans le token." });

                var userId = int.Parse(claim.Value);

                var group = new Group
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    OwnerId = userId,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow
                };

                group.Members.Add(new GroupMember
                {
                    UserId = userId,
                    Role = "Admin",
                    Status = MemberStatus.Accepted,
                    IsAccepted = true
                });

                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    id = group.Id,
                    name = group.Name,
                    inviteCode = group.InviteCode
                });
            }
            catch (Exception ex)
            {
                var trueError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = "Erreur SQL/C# : " + trueError });
            }
        }

        [HttpPost("join/code")]
        public async Task<IActionResult> JoinByCode([FromBody] string code)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.InviteCode == code && !g.IsDeleted);

            if (group == null) return NotFound(new { message = "Code invalide ou groupe supprimé" });

            if (await _context.GroupMembers.AnyAsync(m => m.GroupId == group.Id && m.UserId == userId))
                return BadRequest(new { message = "Vous êtes déjà dans ce groupe ou en attente." });

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = group.Id,
                UserId = userId,
                Role = "Member",
                Status = MemberStatus.Accepted,
                IsAccepted = true
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Groupe rejoint !" });
        }

        [HttpPost("join/request/{groupId}")]
        public async Task<IActionResult> RequestJoin(int groupId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            if (await _context.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId))
                return BadRequest(new { message = "Déjà membre ou demande en cours." });

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                Role = "Member",
                Status = MemberStatus.Pending,
                IsAccepted = false
            });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Demande envoyée." });
        }

        [HttpPost("accept/{memberId}")]
        public async Task<IActionResult> AcceptMember(int memberId)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var memberRequest = await _context.GroupMembers.FirstOrDefaultAsync(m => m.Id == memberId);

            if (memberRequest == null) return NotFound();

            var group = await _context.Groups.FindAsync(memberRequest.GroupId);
            var requesterIsAdmin = await _context.GroupMembers
                .AnyAsync(m => m.GroupId == memberRequest.GroupId && m.UserId == adminId && m.Role == "Admin");

            if (!requesterIsAdmin && group?.OwnerId != adminId)
                return Unauthorized(new { message = "Seul un admin peut accepter." });

            memberRequest.IsAccepted = true;
            memberRequest.Status = MemberStatus.Accepted;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Membre accepté." });
        }

        [HttpGet("my-groups")]
        public async Task<IActionResult> GetMyGroups()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var groups = await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId && m.IsAccepted) && !g.IsDeleted)
                .Select(g => new { g.Id, g.Name, g.InviteCode, g.IconUrl, g.Description })
                .ToListAsync();

            return Ok(groups);
        }

        [HttpGet("{groupId}/messages")]
        public async Task<IActionResult> GetGroupMessages(int groupId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            if (!await _context.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId && m.IsAccepted))
                return Unauthorized();

            var messages = await _context.Messages
                .Where(m => m.GroupId == groupId)
                .OrderBy(m => m.CreatedAt)
                .Select(m => new {
                    m.Id,
                    m.Content,
                    Timestamp = m.CreatedAt,
                    SenderName = m.Sender.Name,
                    SenderId = m.SenderId
                })
                .ToListAsync();

            return Ok(messages);
        }
        [HttpGet("{groupId}/members")]
        public async Task<IActionResult> GetGroupMembers(int groupId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            if (!await _context.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId && m.IsAccepted))
                return Unauthorized();

            var members = await _context.GroupMembers
                .Include(m => m.User)
                .Where(m => m.GroupId == groupId && m.IsAccepted)
                .Select(m => new {
                    UserId = m.UserId,
                    Name = m.User.Name,
                    Role = m.Role
                })
                .ToListAsync();

            return Ok(members);
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var group = await _context.Groups.FindAsync(groupId);

            if (group == null || group.IsDeleted) return NotFound();

            if (group.OwnerId != userId)
                return Unauthorized(new { message = "Seul le créateur du groupe peut le supprimer." });

            group.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Groupe supprimé avec succès." });
        }

        [HttpPost("{groupId}/transfer/{newOwnerId}")]
        public async Task<IActionResult> TransferAdmin(int groupId, int newOwnerId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var group = await _context.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return NotFound();

            if (group.OwnerId != userId)
                return Unauthorized(new { message = "Seul le créateur peut transférer ses droits." });

            var newOwnerMember = group.Members.FirstOrDefault(m => m.UserId == newOwnerId && m.IsAccepted);
            if (newOwnerMember == null)
                return BadRequest(new { message = "L'utilisateur sélectionné n'est pas membre du groupe." });

            var currentOwnerMember = group.Members.FirstOrDefault(m => m.UserId == userId);

            group.OwnerId = newOwnerId;
            newOwnerMember.Role = "Admin";
            if (currentOwnerMember != null) currentOwnerMember.Role = "Member";

            await _context.SaveChangesAsync();
            return Ok(new { message = "Droits transférés avec succès." });
        }
    }

    public class CreateGroupDto { public string Name { get; set; } = null!; public string? Description { get; set; } }
}