namespace STRaceLifePG.ViewModel
{
    public class RaceListViewModel
    {
        public IEnumerable<RaceViewModel> Races { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
