using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface ILiteraturBilesikRepository
    {
        Task<IEnumerable<LiteraturBilesik>> GetAllAsync();
        Task<FilterResponse<LiteraturBilesik>> QueryAsync(FilterRequest request);
        Task<LiteraturBilesik?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturBilesik entity);
        Task UpdateAsync(LiteraturBilesik entity);
        Task DeleteAsync(int id);
    }
}
