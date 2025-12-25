using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitkiController : ControllerBase
    {
        private readonly IBitkiRepository _repository;

        public BitkiController(IBitkiRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> Get()
        {
            var plants = await _repository.GetAllAsync();
            return Ok(plants);
        }
    }
}
