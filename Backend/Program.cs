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
        });

        context.SaveChanges();
    }
}