using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Services;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase // Rename to BitkiController later if requested, but sticking to existing file for now
    {
        private readonly IBitkiService _service;

        public PlantsController(IBitkiService service)
        {
            _service = service;
        }

        // GET: api/plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            var plants = await _service.GetAllPlantsAsync();
            return Ok(plants);
        }

        // GET: api/plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _service.GetPlantByIdAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            return Ok(plant);
        }

        // POST: api/plants
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            var id = await _service.CreatePlantAsync(plant);
            plant.Id = id;
            return CreatedAtAction(nameof(GetPlant), new { id = plant.Id }, plant);
        }

        // PUT: api/plants/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }

            await _service.UpdatePlantAsync(plant);
            return NoContent();
        }

        // DELETE: api/plants/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            await _service.DeletePlantAsync(id);
            return NoContent();
        }
    }
}
