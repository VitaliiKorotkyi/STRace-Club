using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.Models
{
    public class Address
    {
        [Key]
        public Guid AddressId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Capital { get; set; }

        [Required]
        [StringLength(100)]
        public string? City { get; set; }

        [Required]
        [StringLength(200)]
        public string? Street { get; set; }
    }
}
