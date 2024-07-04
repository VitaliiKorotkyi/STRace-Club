using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.Models
{
    public class ClubLike
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClubId { get; set; }

        [ForeignKey("ClubId")]
        public virtual Club Club { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
