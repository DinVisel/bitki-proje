using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface ISehirRepository
    {
        Task<IEnumerable<Sehir>> GetAllAsync();
        Task<Sehir?> GetByIdAsync(int id);
        Task<int> AddAsync(Sehir entity);
        Task UpdateAsync(Sehir entity);
        Task DeleteAsync(int id);
    }
}

