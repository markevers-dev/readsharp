using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = [];
    }
}
