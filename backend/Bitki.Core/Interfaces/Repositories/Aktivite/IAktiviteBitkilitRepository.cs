using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteBitkilitRepository
    {
        Task<IEnumerable<AktiviteBitkilit>> GetAllAsync();
        Task<FilterResponse<AktiviteBitkilit>> QueryAsync(FilterRequest request);
        Task<AktiviteBitkilit?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteBitkilit entity);
        Task UpdateAsync(AktiviteBitkilit entity);
        Task DeleteAsync(int id);
    }
}
