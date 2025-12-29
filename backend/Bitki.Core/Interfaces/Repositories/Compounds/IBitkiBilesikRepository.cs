using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBitkiBilesikRepository
    {
        Task<IEnumerable<BitkiBilesik>> GetAllAsync();
        Task<FilterResponse<BitkiBilesik>> QueryAsync(FilterRequest request);
        Task<BitkiBilesik?> GetByIdAsync(int id);
        Task<int> AddAsync(BitkiBilesik entity);
        Task UpdateAsync(BitkiBilesik entity);
        Task DeleteAsync(int id);
    }
}

