using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly DBContext _context;

        public GenreController(DBContext context)
        {
            _context = context;
        }

        // GET: api/genre
        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _context.Genres.ToList();
            return Ok(genres);
        }
    }
}
