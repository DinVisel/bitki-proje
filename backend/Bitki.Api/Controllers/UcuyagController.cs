using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UcuyagController : ControllerBase
    {
        private readonly IUcuyagService _service;

        public UcuyagController(IUcuyagService service)
        {
            _service = service;
        }
    }
}
