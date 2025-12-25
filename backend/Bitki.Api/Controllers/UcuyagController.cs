using Microsoft.AspNetCore.Mvc;
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
    }
}
