using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteLokaliteRepository
    {
        Task<IEnumerable<AktiviteLokalite>> GetAllAsync();
    }
}
