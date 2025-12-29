using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bitki.Core.Entities;
using Bitki.Core.Models;
using Bitki.Core.Interfaces.Repositories.MasterData;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UlkeController : ControllerBase
    {
        private readonly IUlkeRepository _repository;

        public UlkeController(IUlkeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ulke>>> Get() => Ok(await _repository.GetAllAsync());

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Ulke>>> Query([FromBody] FilterRequest request)
        {
            return Ok(await _repository.QueryAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ulke>> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Create([FromBody] Ulke entity)
        {
            var id = await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Ulke entity)
        {
            if (id != entity.Id) return BadRequest("ID mismatch");
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}

