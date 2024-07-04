using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace STRaceLifePG.Models
{
    public class User : IdentityUser<Guid>
    {
       
        
           

            [Required]
            [StringLength(100)]
            public string Name { get; set; } = null!;

            [Required]
            [EmailAddress]
            public override string Email { get; set; } = null!;

           

            [Required]
            [StringLength(100)]
            public string Location { get; set; } = null!;

            public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
            public virtual ICollection<Race> Races { get; set; } = new List<Race>();
        

    }
}
