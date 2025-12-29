using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteCalismaRepository
    {
        Task<IEnumerable<AktiviteCalisma>> GetAllAsync();
        Task<AktiviteCalisma?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteCalisma entity);
        Task UpdateAsync(AktiviteCalisma entity);
        Task DeleteAsync(int id);
    }
}
