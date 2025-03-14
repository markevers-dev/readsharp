using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController(IGenericRepository<Publisher> publisherRepository) : ControllerBase
    {
        private readonly IGenericRepository<Publisher> _publisherRepository = publisherRepository;

        // GET: api/publisher
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all Publishers.")]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return Ok(publishers);
        }
    }
}
