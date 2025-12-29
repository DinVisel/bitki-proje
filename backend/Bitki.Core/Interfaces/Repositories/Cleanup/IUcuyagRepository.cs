using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IUcuyagRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync();
        Task<Bitki.Core.Entities.Ucuyag?> GetByIdAsync(long id);
        Task<FilterResponse<Bitki.Core.Entities.Ucuyag>> QueryAsync(FilterRequest request);
        Task<long> AddAsync(Bitki.Core.Entities.Ucuyag entity);
        Task UpdateAsync(Bitki.Core.Entities.Ucuyag entity);
        Task DeleteAsync(long id);
    }
}

