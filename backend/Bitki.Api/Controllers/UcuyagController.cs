using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Cleanup;

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
    }
}
