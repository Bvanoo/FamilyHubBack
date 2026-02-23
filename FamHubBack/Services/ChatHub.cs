using Microsoft.AspNetCore.SignalR;
using FamHubBack.Data;
using FamHubBack.Data.Entities;

namespace FamHubBack.Services
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task JoinGroupHub(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task LeaveGroupHub(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task SendMessageToGroup(string groupId, string messageContent, string senderId, string senderName)
        {
            int gId = int.Parse(groupId);
            int sId = int.Parse(senderId);

            var msg = new Message
            {
                GroupId = gId,
                SenderId = sId,
                Content = messageContent,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(groupId).SendAsync("ReceiveMessage", senderName, messageContent, msg.CreatedAt);
        }
    }
}