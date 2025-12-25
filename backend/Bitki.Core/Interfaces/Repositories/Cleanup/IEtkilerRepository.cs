using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IEtkilerRepository
    {
        Task<IEnumerable<Etkiler>> GetAllAsync();
    }
}
