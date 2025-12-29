using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnobitkilitRepository
    {
        Task<IEnumerable<Etnobitkilit>> GetAllAsync();
        Task<Etnobitkilit?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnobitkilit entity);
        Task UpdateAsync(Etnobitkilit entity);
        Task DeleteAsync(int id);
    }
}
