using Microsoft.AspNetCore.Mvc;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces.Repositories.Aktivite;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktiviteSaflastirmaController : ControllerBase
    {
        private readonly IAktiviteSaflastirmaRepository _repository;

        public AktiviteSaflastirmaController(IAktiviteSaflastirmaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AktiviteSaflastirma>>> Get()
        {
            var data = await _repository.GetAllAsync();
            return Ok(data);
        }
    }
}
