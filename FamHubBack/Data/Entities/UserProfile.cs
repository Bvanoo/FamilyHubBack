using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamHubBack.Data.Entities
{
    [Table("Profiles")]
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string Nom { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Prenom { get; set; } = null!;

        public DateTime? DateNaissance { get; set; }

        [MaxLength(255)]
        public string Adresse { get; set; } = null!;

        [MaxLength(20)]
        public string Telephone { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
