using FamHubBack.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamHubBack.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<UserProfile> Profiles => Set<UserProfile>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationMember> ConversationMembers => Set<ConversationMember>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Tricount> Tricounts => Set<Tricount>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ExpenseParticipant> ExpenseParticipants => Set<ExpenseParticipant>();
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<SecretSantaDraw> SecretSantaDraws { get; set; }
        public DbSet<EventTask> EventTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}