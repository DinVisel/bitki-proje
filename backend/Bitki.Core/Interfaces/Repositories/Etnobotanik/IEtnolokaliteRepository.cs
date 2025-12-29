using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnolokaliteRepository
    {
        Task<IEnumerable<Etnolokalite>> GetAllAsync();
        Task<Etnolokalite?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnolokalite entity);
        Task UpdateAsync(Etnolokalite entity);
        Task DeleteAsync(int id);
    }
}
