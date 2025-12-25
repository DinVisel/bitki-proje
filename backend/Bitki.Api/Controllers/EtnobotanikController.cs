using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtnobotanikController : ControllerBase
    {
        private readonly IEtnobotanikService _service;

        public EtnobotanikController(IEtnobotanikService service)
        {
            _service = service;
        }
    }
}
