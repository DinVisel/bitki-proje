using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Aktivite;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktiviteLokaliteController : ControllerBase
    {
        private readonly IAktiviteLokaliteRepository _repository;
        public AktiviteLokaliteController(IAktiviteLokaliteRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AktiviteLokalite>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AktiviteLokalite>> GetById(int id) { var item = await _repository.GetByIdAsync(id); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] AktiviteLokalite entity) { var id = await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { id }, id); }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] AktiviteLokalite entity) { if (id != entity.Id) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) { await _repository.DeleteAsync(id); return NoContent(); }
    }
}
