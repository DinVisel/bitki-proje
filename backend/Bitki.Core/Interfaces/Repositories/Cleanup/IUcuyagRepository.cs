using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IUcuyagRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync();
    }
}
