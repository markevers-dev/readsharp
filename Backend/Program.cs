using Backend;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddControllers();

var app = builder.Build();

var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(webRootPath))
{
    Directory.CreateDirectory(webRootPath);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(webRootPath),
    RequestPath = "/wwwroot"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadSharp API V1");
    });
}

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

static void SeedData(DBContext context)
{
    context.Publishers.RemoveRange(context.Publishers);
    context.Authors.RemoveRange(context.Authors);
    context.Genres.RemoveRange(context.Genres);
    context.Books.RemoveRange(context.Books);

    context.SaveChanges();

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
    },
    new Author
    {
        Name = "Bernard Cornwell",
    },
    new Author
    {
        Name = "William Goldman"
    },
    new Author
    {
        Name = "Travis Baldree",
    },
    new Author
    {
        Name = "John Gwynne"
    },
    new Author
    {
        Name = "Ferdia Lennon",
    },
    new Author
    {
        Name = "R.J. Barker"
    });

    context.SaveChanges();

    context.Genres.AddRange(new Genre
    {
        Name = "Fantasy"
    },
    new Genre
    {
        Name = "Science Fiction"
    },
    new Genre
    {
        Name = "Historical Fiction"
    });

    context.SaveChanges();

    context.Books.AddRange(new Book
    {
        Title = "Harry Potter and the Sorcerer's Stone",
        Description = "A young boy discovers he is a wizard.",
        Price = 19.99m,
        Rating = 5,
        PageCount = 333,
        PublicationYear = 1997,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "J.K. Rowling") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "42844155.jpg"
    },
    new Book
    {
        Title = "A Game of Thrones",
        Description = "A fantasy epic set in the Seven Kingdoms.",
        Price = 29.99m,
        Rating = 4,
        PageCount = 864,
        PublicationYear = 1996,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "George R.R. Martin") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "822993.jpg"
    },
    new Book
    {
        Title = "The Winter King",
        Description = "The first of Bernard Cornwell's Warlord Chronicles, The Winter King is a brilliant retelling of the Arthurian legend, combining myth, history and thrilling battlefield action.",
        Price = 18.99m,
        Rating = 4,
        PageCount = 512,
        PublicationYear = 1994,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Bernard Cornwell") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Historical Fiction") },
        CoverImagePath = "30171946.jpg"
    },
    new Book
    {
        Title = "The Name of the Wind",
        Description = "Told in Kvothe's own voice, this is the tale of the magically gifted young man who grows to be the most notorious wizard his world has ever seen. ",
        Price = 21.99m,
        Rating = 5,
        PageCount = 752,
        PublicationYear = 2009,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Patrick Rothfuss") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "34347493.jpg"
    },
    new Book
    {
        Title = "The Gods Themselves",
        Description = "In the twenty-second century Earth obtains limitless, free energy from a source science little understands: an exchange between Earth and a parallel universe, using a process devised by the aliens. But even free energy has a price. The transference process itself will eventually lead to the destruction of the Earth's Sun--and of Earth itself.",
        Price = 22.99m,
        Rating = 4,
        PageCount = 272,
        PublicationYear = 1972,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Isaac Asimov") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Science Fiction") },
        CoverImagePath = "17736403.jpg"
    },
    new Book
    {
        Title = "Cage of Souls",
        Description = "A fantasy epic set in the Seven Kingdoms.",
        Price = 20.49m,
        Rating = 4,
        PageCount = 602,
        PublicationYear = 2019,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Adrian Tchaikovsky") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Science Fiction") },
        CoverImagePath = "40803025.jpg"
    },
    new Book
    {
        Title = "Alien Clay",
        Description = "On the distant world of Kiln lie the ruins of an alien civilization. It’s the greatest discovery in humanity’s spacefaring history – yet who were its builders and where did they go?",
        Price = 23.49m,
        Rating = 4,
        PageCount = 396,
        PublicationYear = 2024,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Adrian Tchaikovsky") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Science Fiction") },
        CoverImagePath = "195443798.jpg"
    },
    new Book
    {
        Title = "Dune",
        Description = "Set on the desert planet Arrakis, Dune is the story of the boy Paul Atreides, heir to a noble family tasked with ruling an inhospitable world where the only thing of value is the “spice” melange, a drug capable of extending life and enhancing consciousness.",
        Price = 27.99m,
        Rating = 4,
        PageCount = 658,
        PublicationYear = 1965,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Frank Herbert") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Science Fiction") },
        CoverImagePath = "44767458.jpg"
    },
    new Book
    {
        Title = "Legends & Lattes",
        Description = "High Fantasy. Low Stakes. Good Company.",
        Price = 27.99m,
        Rating = 4,
        PageCount = 308,
        PublicationYear = 2022,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Travis Baldree") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "61457585.jpg"
    },
    new Book
    {
        Title = "Dune Messiah",
        Description = "Dune Messiah continues the story of Paul Atreides, better know--and feared--as the the man called Muad'Dib. As Emperor of the known universe, Paul possesses more power than a single man was ever meant to wield.",
        Price = 24.99m,
        Rating = 3,
        PageCount = 336,
        PublicationYear = 1969,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Frank Herbert") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Science Fiction") },
        CoverImagePath = "61403877.jpg"
    },
    new Book
    {
        Title = "The Bone Ships",
        Description = "Two nations at war. A prize beyond compare.",
        Price = 14.99m,
        Rating = 4,
        PageCount = 512,
        PublicationYear = 2019,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "R.J. Barker") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "43521682.jpg"
    },
    new Book
    {
        Title = "Glorious Exploits",
        Description = "An utterly original celebration of that which binds humanity across battle lines and history.",
        Price = 19.99m,
        Rating = 4,
        PageCount = 304,
        PublicationYear = 2024,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "Penguin Random House").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "Ferdia Lennon") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Historical Fiction") },
        CoverImagePath = "127278133.jpg"
    },
    new Book
    {
        Title = "The Princess Bride",
        Description = "The Princess Bride is a true fantasy classic. William Goldman describes it as a \"good parts version\" of \"S. Morgenstern's Classic Tale of True Love and High Adventure.\"",
        Price = 16.99m,
        Rating = 4,
        PageCount = 465,
        PublicationYear = 1973,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "William Goldman") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "9525672.jpg"
    },
    new Book
    {
        Title = "The Shadow of the Gods",
        Description = "After the gods warred and drove themselves to extinction, the cataclysm of their fall shattered the land of Vigrið.",
        Price = 18.99m,
        Rating = 4,
        PageCount = 480,
        PublicationYear = 2021,
        PublisherId = context.Publishers.FirstOrDefault(p => p.Name == "HarperCollins").Id,
        Authors = new List<Author> { context.Authors.FirstOrDefault(a => a.Name == "John Gwynne") },
        Genres = new List<Genre> { context.Genres.FirstOrDefault(g => g.Name == "Fantasy") },
        CoverImagePath = "52694527.jpg"
    });

    context.SaveChanges();
}