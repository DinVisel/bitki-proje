using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteLokaliteRepository
    {
        Task<IEnumerable<AktiviteLokalite>> GetAllAsync();
        Task<FilterResponse<AktiviteLokalite>> QueryAsync(FilterRequest request);
        Task<AktiviteLokalite?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteLokalite entity);
        Task UpdateAsync(AktiviteLokalite entity);
        Task DeleteAsync(int id);
    }
}
