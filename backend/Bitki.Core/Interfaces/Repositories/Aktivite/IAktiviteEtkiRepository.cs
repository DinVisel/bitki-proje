using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteEtkiRepository
    {
        Task<IEnumerable<AktiviteEtki>> GetAllAsync();
        Task<AktiviteEtki?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteEtki entity);
        Task UpdateAsync(AktiviteEtki entity);
        Task DeleteAsync(int id);
    }
}
