using FamHubBack.Data;
using FamHubBack.Data.Entities;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext db;

    public ChatHub(ApplicationDbContext context)
    {
        db = context;
    }
    public async Task SendMessage(int conversationId, int senderId, string content)
    {
        var msg = new Message
        {
            Content = content,
            SenderId = senderId,
            ConversationId = conversationId,
            CreatedAt = DateTime.UtcNow
        };

        db.Messages.Add(msg);
        await db.SaveChangesAsync();
        await Clients.Group($"conv_{conversationId}")
                     .SendAsync("ReceiveMessage", senderId, content, msg.CreatedAt);
    }
    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv_{conversationId}");
    }
}