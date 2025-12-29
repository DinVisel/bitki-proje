using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteCalismaRepository
    {
        Task<IEnumerable<AktiviteCalisma>> GetAllAsync();
        Task<FilterResponse<AktiviteCalisma>> QueryAsync(FilterRequest request);
        Task<AktiviteCalisma?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteCalisma entity);
        Task UpdateAsync(AktiviteCalisma entity);
        Task DeleteAsync(int id);
    }
}
