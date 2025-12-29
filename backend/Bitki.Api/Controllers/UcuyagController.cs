using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Cleanup;
using Bitki.Core.Models;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UcuyagController : ControllerBase
    {
        private readonly IUcuyagRepository _repository;

        public UcuyagController(IUcuyagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bitki.Core.Entities.Ucuyag>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bitki.Core.Entities.Ucuyag>> GetById(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Bitki.Core.Entities.Ucuyag>>> Query([FromBody] FilterRequest request)
        {
            try
            {
                var result = await _repository.QueryAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<long>> Create([FromBody] Bitki.Core.Entities.Ucuyag entity)
        {
            var id = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] Bitki.Core.Entities.Ucuyag entity)
        {
            if (id != entity.Id)
            {
                return BadRequest("ID mismatch");
            }
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}

