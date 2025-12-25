using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Cleanup;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtkilerController : ControllerBase
    {
        private readonly IEtkilerRepository _repository;

        public EtkilerController(IEtkilerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Etkiler>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }
    }
}
