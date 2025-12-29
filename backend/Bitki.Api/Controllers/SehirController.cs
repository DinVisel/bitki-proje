using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.MasterData;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SehirController : ControllerBase
    {
        private readonly ISehirRepository _repository;
        public SehirController(ISehirRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sehir>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Sehir>> GetById(int id) { var item = await _repository.GetByIdAsync(id); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] Sehir entity) { var id = await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { id }, id); }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Sehir entity) { if (id != entity.Id) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) { await _repository.DeleteAsync(id); return NoContent(); }
    }
}

