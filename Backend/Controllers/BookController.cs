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
