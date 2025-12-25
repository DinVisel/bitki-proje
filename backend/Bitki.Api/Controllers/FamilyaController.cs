using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Taxonomy;
using Bitki.Core.Models;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyaController : ControllerBase
    {
        private readonly IFamilyaRepository _repository;

        public FamilyaController(IFamilyaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Familya>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }

        [HttpPost("query")]
        public async Task<ActionResult<FilterResponse<Familya>>> Query([FromBody] FilterRequest request)
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
