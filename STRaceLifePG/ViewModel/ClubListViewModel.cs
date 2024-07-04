using STRaceLifePG.Models;

namespace STRaceLifePG.ViewModel
{
    public class ClubListViewModel
    {
        public List<ClubViewModel> Clubs { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
