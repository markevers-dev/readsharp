namespace Backend.Data
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
        public int? PublicationYear { get; set; }
        public string PublisherName { get; set; } = null!;
        public string CoverImagePath { get; set; } = null!;
        public List<string> AuthorNames { get; set; } = null!;
        public List<string> GenreNames { get; set; } = null!;
    }
}
