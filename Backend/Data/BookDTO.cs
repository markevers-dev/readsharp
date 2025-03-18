namespace Backend.Data
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int PageCount { get; set; }
        public int Rating { get; set; }
        public int? PublicationYear { get; set; }
        public PublisherDTO Publisher { get; set; } = null!;
        public string CoverImagePath { get; set; } = null!;
        public List<AuthorDTO> Authors { get; set; } = null!;
        public List<GenreDTO> Genres { get; set; } = null!;
    }
}
