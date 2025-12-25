using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;

namespace Bitki.Infrastructure.Services
{
    public class EtnobotanikService : IEtnobotanikService
    {
        private readonly IEtnobitkilitRepository _repository;

        public EtnobotanikService(IEtnobitkilitRepository repository)
        {
            _repository = repository;
        }
    }
}
