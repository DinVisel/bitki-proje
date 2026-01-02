
using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Herbarium
{
    public interface IHerbariumRepository
    {
        Task<Entities.Herbarium?> GetByIdAsync(int id);
        Task<int> AddAsync(Entities.Herbarium entity);
        Task UpdateAsync(Entities.Herbarium entity);
        Task DeleteAsync(int id);
        Task<FilterResponse<Entities.Herbarium>> QueryAsync(FilterRequest request);
    }
}
