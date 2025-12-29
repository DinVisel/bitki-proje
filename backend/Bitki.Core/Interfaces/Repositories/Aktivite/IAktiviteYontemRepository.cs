using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteYontemRepository
    {
        Task<IEnumerable<AktiviteYontem>> GetAllAsync();
        Task<AktiviteYontem?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteYontem entity);
        Task UpdateAsync(AktiviteYontem entity);
        Task DeleteAsync(int id);
    }
}
