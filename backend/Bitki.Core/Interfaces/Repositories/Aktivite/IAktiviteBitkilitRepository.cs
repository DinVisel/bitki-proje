using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteBitkilitRepository
    {
        Task<IEnumerable<AktiviteBitkilit>> GetAllAsync();
    }
}
