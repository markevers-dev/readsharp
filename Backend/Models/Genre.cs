﻿using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = [];
    }
}
