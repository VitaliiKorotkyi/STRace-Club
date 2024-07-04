using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.Models
{
    public class Club
    {
        [Key]
        public Guid ClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<Race> Races { get; set; } = new List<Race>();

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public virtual ICollection<ClubLike> ClubLikes { get; set; } = new List<ClubLike>();

        [NotMapped]
        public int LikeCount { get; set; }


    }
}
