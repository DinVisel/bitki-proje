using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Ucuyag;

namespace Bitki.Infrastructure.Services
{
    public class UcuyagService : IUcuyagService
    {
        private readonly IUcuyagRepository _repository;

        public UcuyagService(IUcuyagRepository repository)
        {
            _repository = repository;
        }
    }
}
