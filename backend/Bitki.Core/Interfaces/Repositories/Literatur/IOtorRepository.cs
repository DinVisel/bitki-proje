using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface IOtorRepository
    {
        Task<IEnumerable<Otor>> GetAllAsync();
        Task<FilterResponse<Otor>> QueryAsync(FilterRequest request);
        Task<Otor?> GetByIdAsync(long id);
        Task<long> AddAsync(Otor entity);
        Task UpdateAsync(Otor entity);
        Task DeleteAsync(long id);
    }
}

