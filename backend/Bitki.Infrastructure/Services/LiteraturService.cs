using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Literatur;

namespace Bitki.Infrastructure.Services
{
    public class LiteraturService : ILiteraturService
    {
        private readonly ILiteraturRepository _repository;

        public LiteraturService(ILiteraturRepository repository)
        {
            _repository = repository;
        }
    }
}
