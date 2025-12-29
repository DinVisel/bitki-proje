using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Literatur>> GetAllAsync();
        Task<Bitki.Core.Entities.Literatur?> GetByIdAsync(long id);
        Task<FilterResponse<Bitki.Core.Entities.Literatur>> QueryAsync(FilterRequest request);
        Task<long> AddAsync(Bitki.Core.Entities.Literatur entity);
        Task UpdateAsync(Bitki.Core.Entities.Literatur entity);
        Task DeleteAsync(long id);
    }
}

