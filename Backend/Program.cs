using System;
using Backend;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DBContext>();

    context.Database.Migrate();

    SeedData(context);
}

app.UseAuthorization();

app.MapControllers();

app.Run();


void SeedData(DBContext context)
{
    if (!context.Publishers.Any())
    {
        context.Publishers.AddRange(new Publisher
        {
            Name = "HarperCollins",
            Address = "195 Broadway, New York, NY",
        },
        new Publisher
        {
            Name = "Penguin Random House",
            Address = "1745 Broadway, New York, NY",
        });

        context.SaveChanges();
    }

    if (!context.Authors.Any())
    {
        context.Authors.AddRange(new Author
        {
            Name = "J.K. Rowling",
        },
        new Author
        {
            Name = "George R.R. Martin",
        },
        new Author
        {
            Name = "Patrick Rothfuss",
        },
        new Author
        {
            Name = "Isaac Asimov",
        },
        new Author
        {
            Name = "Frank Herbert",
        },
        new Author
        {
            Name = "Adrian Tchaikovsky",
        },
        new Author
        {
            Name = "R.J. Barker"
        });

        context.SaveChanges();
    }

    if (!context.Genres.Any())
    {
        context.Genres.AddRange(new Genre
        {
            Name = "Fantasy"
        },
        new Genre
        {
            Name = "Science Fiction"
        });

        context.SaveChanges();
    }

    if (!context.Books.Any())
    {
        context.Books.AddRange(new Book
        {
            Title = "Harry Potter and the Sorcerer's Stone",
            Description = "A young boy discovers he is a wizard.",
            Price = 19.99m,
            Rating = 5,
            PublicationYear = 1997,
            PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
            Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "J.K. Rowling") },
            Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") }
        },
        new Book
        {
            Title = "A Game of Thrones",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 29.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        },
        new Book
        {
            Title = "A Game of Thrones",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 29.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        },
        new Book
        {
            Title = "A Game of Thrones",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 29.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        },
        new Book
        {
            Title = "A Game of Thrones",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 29.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        },
        new Book
        {
            Title = "Dune",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 24.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        },
        new Book
        {
            Title = "A Game of Thrones",
            Description = "A fantasy epic set in the Seven Kingdoms.",
            Price = 29.99m,
            Rating = 4,
            PublicationYear = 1996,
            PublisherId = context.Publishers.Last().Id,
            Authors = new List<Author> { context.Authors.Last() },
            Genres = new List<Genre> { context.Genres.First() }
        });

        context.SaveChanges();
    }
}