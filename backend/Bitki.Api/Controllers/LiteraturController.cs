using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteraturController : ControllerBase
    {
        private readonly ILiteraturService _service;

        public LiteraturController(ILiteraturService service)
        {
            _service = service;
        }
    }
}
