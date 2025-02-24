using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public ICollection<Book> Books { get; set; } = [];
    }
}
