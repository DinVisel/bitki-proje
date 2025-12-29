using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteTestYeriRepository
    {
        Task<IEnumerable<AktiviteTestYeri>> GetAllAsync();
        Task<AktiviteTestYeri?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteTestYeri entity);
        Task UpdateAsync(AktiviteTestYeri entity);
        Task DeleteAsync(int id);
    }
}
