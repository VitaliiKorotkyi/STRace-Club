using STRaceLifePG.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.Models
{
    public class Race
    {
        [Key]
        public Guid RaceId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public RaceCategoryEnum Category { get; set; }

        [Required]
        public Guid ClubId { get; set; }

        [ForeignKey("ClubId")]
        public virtual Club? Club { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string? Capital { get; set; }

        [Required]
        [StringLength(100)]
        public string? City { get; set; }

        [Required]
        [StringLength(200)]
        public string? Street { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public DateTime StartDate { get; set; }
    }
}
