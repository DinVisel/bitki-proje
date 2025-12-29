using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.MasterData;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KisilerController : ControllerBase
    {
        private readonly IKisilerRepository _repository;
        public KisilerController(IKisilerRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kisiler>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Kisiler>> GetById(long id) { var item = await _repository.GetByIdAsync(id); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> Create([FromBody] Kisiler entity) { var id = await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { id }, id); }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] Kisiler entity) { if (id != entity.Id) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id) { await _repository.DeleteAsync(id); return NoContent(); }
    }
}

