using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Cleanup;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitkiResimleriController : ControllerBase
    {
        private readonly IBitkiResimleriRepository _repository;

        public BitkiResimleriController(IBitkiResimleriRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BitkiResimleri>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("plant/{plantId}")]
        public async Task<ActionResult<IEnumerable<BitkiResimleri>>> GetByPlantId(int plantId)
        {
            var images = await _repository.GetByPlantIdAsync(plantId);
            return Ok(images);
        }
    }
}
