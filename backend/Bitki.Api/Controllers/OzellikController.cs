using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Cleanup;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OzellikController : ControllerBase
    {
        private readonly IOzellikRepository _repository;

        public OzellikController(IOzellikRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bitki.Core.Entities.Ozellik>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }
    }
}
