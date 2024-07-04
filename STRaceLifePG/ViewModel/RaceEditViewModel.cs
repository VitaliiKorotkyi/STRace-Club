using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using STRaceLifePG.Enum;

namespace STRaceLifePG.ViewModel
{
    public class RaceEditViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string? Capital { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public RaceCategoryEnum Category { get; set; }
    }
}
