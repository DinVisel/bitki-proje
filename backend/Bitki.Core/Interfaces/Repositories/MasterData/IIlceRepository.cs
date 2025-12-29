using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IIlceRepository
    {
        Task<IEnumerable<Ilce>> GetAllAsync();
        Task<Ilce?> GetByIdAsync(int id);
        Task<int> AddAsync(Ilce entity);
        Task UpdateAsync(Ilce entity);
        Task DeleteAsync(int id);
    }
}

