using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface ILiteraturBilesikRepository
    {
        Task<IEnumerable<LiteraturBilesik>> GetAllAsync();
        Task<LiteraturBilesik?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturBilesik entity);
        Task UpdateAsync(LiteraturBilesik entity);
        Task DeleteAsync(int id);
    }
}
