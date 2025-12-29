using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteLokaliteRepository
    {
        Task<IEnumerable<AktiviteLokalite>> GetAllAsync();
        Task<AktiviteLokalite?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteLokalite entity);
        Task UpdateAsync(AktiviteLokalite entity);
        Task DeleteAsync(int id);
    }
}
