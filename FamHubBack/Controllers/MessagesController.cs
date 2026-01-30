using FamHubBack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamHubBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MessagesController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetHistory(int conversationId)
        {
            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .Select(m => new {
                    m.SenderId,
                    m.Content,
                    m.CreatedAt
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}