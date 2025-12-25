using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktiviteController : ControllerBase
    {
        private readonly IAktiviteService _service;

        public AktiviteController(IAktiviteService service)
        {
            _service = service;
        }
    }
}
