using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Compounds;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UcucuYagBilesikController : ControllerBase
    {
        private readonly IUcucuYagBilesikRepository _repository;
        public UcucuYagBilesikController(IUcucuYagBilesikRepository repository) { _repository = repository; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UcucuYagBilesik>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpGet("{essentialOilId}/{compoundId}")]
        public async Task<ActionResult<UcucuYagBilesik>> GetById(int essentialOilId, int compoundId) { var item = await _repository.GetByIdAsync(essentialOilId, compoundId); return item == null ? NotFound() : Ok(item); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UcucuYagBilesik entity) { await _repository.AddAsync(entity); return CreatedAtAction(nameof(GetById), new { essentialOilId = entity.EssentialOilId, compoundId = entity.CompoundId }, entity); }

        [HttpPut("{essentialOilId}/{compoundId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int essentialOilId, int compoundId, [FromBody] UcucuYagBilesik entity) { if (essentialOilId != entity.EssentialOilId || compoundId != entity.CompoundId) return BadRequest("ID mismatch"); await _repository.UpdateAsync(entity); return NoContent(); }

        [HttpDelete("{essentialOilId}/{compoundId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int essentialOilId, int compoundId) { await _repository.DeleteAsync(essentialOilId, compoundId); return NoContent(); }
    }
}

