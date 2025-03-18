namespace Frontend.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int PageCount { get; set; }

        public int Rating { get; set; }

        public int? PublicationYear { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public string CoverImagePath { get; set; } = null!;

        public ICollection<Author> Authors { get; set; } = [];
        public ICollection<Genre> Genres { get; set; } = [];
    }
}
