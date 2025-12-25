using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteSaflastirmaRepository
    {
        Task<IEnumerable<AktiviteSaflastirma>> GetAllAsync();
    }
}
