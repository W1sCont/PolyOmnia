namespace PolyOmnia.Models
{
    public class HomeViewModel
    {
        public Video? HeroVideo { get; set; }
        public IEnumerable<Video> Videos { get; set; } = new List<Video>();
        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
        public int? SelectedGenreId { get; set; }
    }
}