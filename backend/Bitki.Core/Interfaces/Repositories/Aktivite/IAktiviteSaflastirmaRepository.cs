using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteSaflastirmaRepository
    {
        Task<IEnumerable<AktiviteSaflastirma>> GetAllAsync();
        Task<AktiviteSaflastirma?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteSaflastirma entity);
        Task UpdateAsync(AktiviteSaflastirma entity);
        Task DeleteAsync(int id);
    }
}
