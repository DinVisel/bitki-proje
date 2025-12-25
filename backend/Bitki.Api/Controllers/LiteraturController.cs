using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Literatur;
using Bitki.Core.Models;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteraturController : ControllerBase
    {
        private readonly ILiteraturRepository _repository;

        public LiteraturController(ILiteraturRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bitki.Core.Entities.Literatur>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Bitki.Core.Entities.Literatur>>> Query([FromBody] FilterRequest request)
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
    }
}
