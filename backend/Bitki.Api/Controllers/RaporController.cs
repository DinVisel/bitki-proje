using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaporController : ControllerBase
    {
        private readonly IRaporService _service;

        public RaporController(IRaporService service)
        {
            _service = service;
        }
    }
}
