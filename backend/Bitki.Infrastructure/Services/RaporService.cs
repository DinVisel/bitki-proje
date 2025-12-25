using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Rapor;

namespace Bitki.Infrastructure.Services
{
    public class RaporService : IRaporService
    {
        private readonly IRaporRepository _repository;

        public RaporService(IRaporRepository repository)
        {
            _repository = repository;
        }
    }
}
