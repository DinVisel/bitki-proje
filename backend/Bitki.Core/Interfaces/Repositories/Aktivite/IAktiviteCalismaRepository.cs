using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteCalismaRepository
    {
        Task<IEnumerable<AktiviteCalisma>> GetAllAsync();
    }
}
