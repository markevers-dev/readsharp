using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly DBContext _context;

        public AuthorController(DBContext context)
        {
            _context = context;
        }

        // GET: api/author
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _context.Authors.ToList();
            return Ok(authors);
        }
    }
}
