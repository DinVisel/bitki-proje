using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteYontemRepository
    {
        Task<IEnumerable<AktiviteYontem>> GetAllAsync();
        Task<FilterResponse<AktiviteYontem>> QueryAsync(FilterRequest request);
        Task<AktiviteYontem?> GetByIdAsync(int id);
        Task<int> AddAsync(AktiviteYontem entity);
        Task UpdateAsync(AktiviteYontem entity);
        Task DeleteAsync(int id);
    }
}
