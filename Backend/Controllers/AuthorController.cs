using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(IGenericRepository<Author> authorRepository) : ControllerBase
    {
        private readonly IGenericRepository<Author> _authorRepository = authorRepository;

        // GET: api/author
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all Authors.")]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return Ok(authors);
        }
    }
}
