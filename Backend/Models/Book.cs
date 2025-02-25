using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public int? PublicationYear { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [MaxLength(500)]
        public string CoverImagePath { get; set; } = null!;

        public ICollection<Author> Authors { get; set; } = [];
        public ICollection<Genre> Genres { get; set; } = [];
    }
}
