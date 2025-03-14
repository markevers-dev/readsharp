using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(IGenericRepository<Genre> genreRepository) : ControllerBase
    {
        private readonly IGenericRepository<Genre> _genreRepository = genreRepository;

        // GET: api/genre
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all Genres.")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreRepository.GetAllAsync();
            return Ok(genres);
        }
    }
}
