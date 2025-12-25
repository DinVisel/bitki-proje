using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OzellikController : ControllerBase
    {
        private readonly IOzellikService _service;

        public OzellikController(IOzellikService service)
        {
            _service = service;
        }
    }
}
