using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Aktivite;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktiviteSaflastirmaController : ControllerBase
    {
        private readonly IAktiviteSaflastirmaRepository _repository;
        public AktiviteSaflastirmaController(IAktiviteSaflastirmaRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AktiviteSaflastirma>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<AktiviteSaflastirma>> GetById(int id) { var item = await _repository.GetByIdAsync(id); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] AktiviteSaflastirma entity) { var id = await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { id }, id); }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] AktiviteSaflastirma entity) { if (id != entity.Id) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) { await _repository.DeleteAsync(id); return NoContent(); }
    }
}
