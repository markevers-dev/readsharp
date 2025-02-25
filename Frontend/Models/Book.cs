namespace Frontend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
        public int? PublicationYear { get; set; }
        public Publisher Publisher { get; set; }
    }
}
