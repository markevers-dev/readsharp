using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = [];
    }
}
