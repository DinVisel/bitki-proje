using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IGenusRepository
    {
        Task<IEnumerable<Genus>> GetAllAsync();
        Task<Genus?> GetByIdAsync(int id);
        Task<FilterResponse<Genus>> QueryAsync(FilterRequest request);
        Task<int> AddAsync(Genus entity);
        Task UpdateAsync(Genus entity);
        Task DeleteAsync(int id);
    }
}

