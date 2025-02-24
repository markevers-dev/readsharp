using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly DBContext _context;

        public PublisherController(DBContext context)
        {
            _context = context;
        }

        // GET: api/publisher
        [HttpGet]
        public IActionResult GetPublishers()
        {
            var publishers = _context.Publishers.ToList();
            return Ok(publishers);
        }
    }
}
