using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
