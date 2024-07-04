using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.ViewModel
{
    public class CreateClubViewModel
    {
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public IFormFile? Image { get; set; }
    }
}
