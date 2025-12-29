using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnolokaliteRepository
    {
        Task<IEnumerable<Etnolokalite>> GetAllAsync();
        Task<FilterResponse<Etnolokalite>> QueryAsync(FilterRequest request);
        Task<Etnolokalite?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnolokalite entity);
        Task UpdateAsync(Etnolokalite entity);
        Task DeleteAsync(int id);
    }
}
