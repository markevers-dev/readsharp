namespace Frontend.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public ICollection<Book> Books { get; set; } = [];
    }
}
