using FamHubBack.Data;
using Microsoft.AspNetCore.Mvc;

namespace FamHubBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("balance/{eventId}")]
        public Dictionary<string, decimal> CalculateBalance(int eventId)
        {
            var expenses = _context.Expenses
                .Where(e => e.CalendarEventId == eventId)
                .ToList();

            var participants = _context.EventParticipants
                .Where(p => p.CalendarEventId == eventId && p.Status == "Present")
                .ToList();

            if (participants.Count == 0) return new Dictionary<string, decimal>();

            decimal totalCost = expenses.Sum(e => e.AmountTotal);
            decimal costPerPerson = totalCost / participants.Count;

            var balance = new Dictionary<string, decimal>();

            foreach (var participant in participants)
            {
                decimal paid = expenses.Where(e => e.PaidBy == participant.UserId).Sum(e => e.AmountTotal);
                balance[participant.UserId.ToString()] = paid - costPerPerson;
            }

            return balance;
        }
    }
}