using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBitkiBilesikRepository
    {
        Task<IEnumerable<BitkiBilesik>> GetAllAsync();
        Task<BitkiBilesik?> GetByIdAsync(int id);
        Task<int> AddAsync(BitkiBilesik entity);
        Task UpdateAsync(BitkiBilesik entity);
        Task DeleteAsync(int id);
    }
}

