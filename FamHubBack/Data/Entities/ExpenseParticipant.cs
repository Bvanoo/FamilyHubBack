using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class ExpenseParticipant
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShareAmount { get; set; }
        public int UserId { get; set; }
        public int ExpenseId { get; set; }
        public User User { get; set; } = null!;
        public Expense Expense { get; set; } = null!;
    }
}
