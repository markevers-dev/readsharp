using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IGenericRepository<Book> bookRepository, IBookRepository bookImageRepository) : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepository = bookRepository;
        private readonly IBookRepository _bookImageRepository = bookImageRepository;

        // GET: api/book
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            return Ok(books);
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            return Ok(book);
        }

        // POST: api/book
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest("Book cannot be null.");

            await _bookRepository.AddAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || id != book.Id)
                return BadRequest("Book data is invalid or ID mismatch.");

            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return NotFound($"Book with ID {id} not found.");

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;
            existingBook.Rating = book.Rating;
            existingBook.PublicationYear = book.PublicationYear;
            existingBook.PublisherId = book.PublisherId;
            existingBook.CoverImagePath = book.CoverImagePath;

            await _bookRepository.UpdateAsync(existingBook);

            return NoContent();
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return NotFound($"Book with ID {id} not found.");

            await _bookRepository.DeleteAsync(id);

            return NoContent();
        }

        // GET: api/book/{id}/cover
        [HttpGet("{id}/cover")]
        public async Task<IActionResult> GetCoverImage(int id)
        {
            try
            {
                var (imageBytes, contentType) = await _bookImageRepository.GetCoverImageAsync(id);

                if (imageBytes == null || string.IsNullOrEmpty(contentType))
                    return NotFound($"No cover image found for book ID {id}.");

                Random random = new();
                int delay = random.Next(200, 1000);
                Thread.Sleep(delay);

                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/book/cover/{existingFileName}
        [HttpPut("cover/{existingFileName}")]
        public async Task<IActionResult> UpdateCover(string existingFileName, IFormFile file)
        {
            try
            {
                var imagePath = await _bookImageRepository.UpdateCoverImageAsync(existingFileName, file);
                return Ok(new { ImagePath = imagePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/book/cover
        [HttpPost("cover")]
        public async Task<IActionResult> UploadCover(IFormFile file)
        {
            try
            {
                var imagePath = await _bookImageRepository.UploadCoverImageAsync(file);
                return Ok(new { ImagePath = imagePath });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private (Boolean, string) IsBookValid(Book book)
        {
            return (true, "yippie");
        }
    }
}
