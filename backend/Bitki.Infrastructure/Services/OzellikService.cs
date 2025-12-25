using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Ozellik;

namespace Bitki.Infrastructure.Services
{
    public class OzellikService : IOzellikService
    {
        private readonly IOzellikRepository _repository;

        public OzellikService(IOzellikRepository repository)
        {
            _repository = repository;
        }
    }
}
