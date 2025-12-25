using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetById(int id)
        {
            var plant = await _repository.GetByIdAsync(id);
            if (plant == null)
                return NotFound();
            return Ok(plant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] Plant plant)
        {
            var id = await _repository.AddAsync(plant);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Plant plant)
        {
            plant.Id = id;
            await _repository.UpdateAsync(plant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}

