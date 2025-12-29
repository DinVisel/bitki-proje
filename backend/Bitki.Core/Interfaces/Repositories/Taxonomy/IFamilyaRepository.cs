using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IFamilyaRepository
    {
        Task<IEnumerable<Familya>> GetAllAsync();
        Task<Familya?> GetByIdAsync(int id);
        Task<FilterResponse<Familya>> QueryAsync(FilterRequest request);
        Task<int> AddAsync(Familya entity);
        Task UpdateAsync(Familya entity);
        Task DeleteAsync(int id);
    }
}

