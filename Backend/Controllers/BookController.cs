using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DBContext _context;

        public BookController(DBContext context)
        {
            _context = context;
        }

        // GET: api/book
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _context.Books.Include(b => b.Authors).Include(b => b.Genres).ToList();
            return Ok(books);
        }

        // GET: api/book/filter?authorId=1&publisherId=2&genreId=3
        [HttpGet("filter")]
        public IActionResult GetBooksByFilters(int? authorId, int? publisherId, int? genreId)
        {
            var query = _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (authorId.HasValue)
            {
                query = query.Where(b => b.Authors.Any(a => a.Id == authorId));
            }

            if (publisherId.HasValue)
            {
                query = query.Where(b => b.PublisherId == publisherId);
            }

            if (genreId.HasValue)
            {
                query = query.Where(b => b.Genres.Any(g => g.Id == genreId));
            }

            var books = query.ToList();

            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _context.Books.Include(b => b.Authors).Include(b => b.Genres).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book cannot be null.");
            }

            if (string.IsNullOrEmpty(book.Title))
            {
                return BadRequest("Book title is required.");
            }
            // TODO: Price, Rating, PublisherId IsNullOrEmpty

            _context.Books.Add(book);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        // POST: api/book/{id}/upload-cover
        [HttpPost("{id}/upload-cover")]
        public async Task<IActionResult> UploadCover(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var allowedMimeTypes = new List<string> { "image/png", "image/webp" };
            var allowedExtensions = new List<string> { ".png", ".webp" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(file.ContentType))
            {
                return BadRequest("Only PNG and WebP images are allowed.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            var filePath = Path.Combine("wwwroot/images", $"{Guid.NewGuid()}{fileExtension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            book.CoverImagePath = filePath;
            await _context.SaveChangesAsync();

            return Ok(new { ImagePath = filePath });
        }


        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || id != book.Id)
            {
                return BadRequest("Book data is invalid or ID mismatch.");
            }

            var existingBook = _context.Books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;
            existingBook.Rating = book.Rating;
            existingBook.PublicationYear = book.PublicationYear;
            existingBook.PublisherId = book.PublisherId;

            _context.Books.Update(existingBook);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT: api/book/{id}/upload-cover
        [HttpPut("{id}/upload-cover")]
        public async Task<IActionResult> UpdateCover(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var allowedMimeTypes = new List<string> { "image/png", "image/webp" };
            var allowedExtensions = new List<string> { ".png", ".webp" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(file.ContentType))
            {
                return BadRequest("Only PNG and WebP images are allowed.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            if (!string.IsNullOrEmpty(book.CoverImagePath))
            {
                var oldFilePath = Path.Combine("wwwroot", book.CoverImagePath);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var newFileName = $"{Guid.NewGuid()}{fileExtension}";
            var newFilePath = Path.Combine("wwwroot/images", newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            book.CoverImagePath = $"images/{newFileName}";
            await _context.SaveChangesAsync();

            return Ok(new { ImagePath = book.CoverImagePath });
        }


        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
