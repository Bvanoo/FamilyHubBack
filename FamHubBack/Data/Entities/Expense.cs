using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountTotal { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int PaidBy { get; set; }
        [ForeignKey("PaidBy")]
        public User Payer { get; set; } = null!;

        public int? TricountId { get; set; }
        public Tricount? Tricount { get; set; }
        public int? CalendarEventId { get; set; }
        [ForeignKey("CalendarEventId")]
        public CalendarEvent? CalendarEvent { get; set; }
    }
}