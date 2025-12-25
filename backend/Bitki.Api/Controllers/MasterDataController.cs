using Bitki.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bitki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _service;

        public MasterDataController(IMasterDataService service)
        {
            _service = service;
        }
    }
}
