using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteBitkilitRepository
    {
        Task<IEnumerable<AktiviteBitkilit>> GetAllAsync();
        Task<AktiviteBitkilit?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteBitkilit entity);
        Task UpdateAsync(AktiviteBitkilit entity);
        Task DeleteAsync(int id);
    }
}
