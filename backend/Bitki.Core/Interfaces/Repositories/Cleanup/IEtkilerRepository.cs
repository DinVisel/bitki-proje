using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IEtkilerRepository
    {
        Task<IEnumerable<Etkiler>> GetAllAsync();
        Task<FilterResponse<Etkiler>> QueryAsync(FilterRequest request);
        Task<Etkiler?> GetByIdAsync(int id);
        Task<int> AddAsync(Etkiler entity);
        Task UpdateAsync(Etkiler entity);
        Task DeleteAsync(int id);
    }
}

