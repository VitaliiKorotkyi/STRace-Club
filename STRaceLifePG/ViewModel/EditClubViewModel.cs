namespace STRaceLifePG.ViewModel
{
    public class EditClubViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public IFormFile Image { get; set; }
    }
}