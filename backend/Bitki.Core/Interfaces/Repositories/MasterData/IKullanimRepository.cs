using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKullanimRepository
    {
        Task<IEnumerable<Kullanim>> GetAllAsync();
        Task<Kullanim?> GetByIdAsync(int id);
        Task<int> AddAsync(Kullanim entity);
        Task UpdateAsync(Kullanim entity);
        Task DeleteAsync(int id);
    }
}

