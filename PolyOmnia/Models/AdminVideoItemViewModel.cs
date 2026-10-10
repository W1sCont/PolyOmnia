namespace PolyOmnia.Models
{
    public class AdminVideoItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ThumbnailUrlOrPath { get; set; }
        public string Duration { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;
    }
}