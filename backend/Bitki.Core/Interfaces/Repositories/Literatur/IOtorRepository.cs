using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface IOtorRepository
    {
        Task<IEnumerable<Otor>> GetAllAsync();
    }
}
