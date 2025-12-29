using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IUlkeRepository
    {
        Task<IEnumerable<Ulke>> GetAllAsync();
        Task<Ulke?> GetByIdAsync(int id);
        Task<int> AddAsync(Ulke entity);
        Task UpdateAsync(Ulke entity);
        Task DeleteAsync(int id);
    }
}

