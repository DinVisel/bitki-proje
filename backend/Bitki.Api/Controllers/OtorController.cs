using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtorController : ControllerBase
    {
        private readonly IOtorRepository _repository;
        public OtorController(IOtorRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Otor>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Otor>>> Query([FromBody] FilterRequest request)
        {
            try { return Ok(await _repository.QueryAsync(request)); }
            catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Otor>> GetById(long id) { var item = await _repository.GetByIdAsync(id); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> Create([FromBody] Otor entity) { var id = await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { id }, id); }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] Otor entity) { if (id != entity.Id) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id) { await _repository.DeleteAsync(id); return NoContent(); }
    }
}
