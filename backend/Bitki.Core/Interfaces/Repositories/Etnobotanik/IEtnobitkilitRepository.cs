using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnobitkilitRepository
    {
        Task<IEnumerable<Etnobitkilit>> GetAllAsync();
        Task<FilterResponse<Etnobitkilit>> QueryAsync(FilterRequest request);
        Task<Etnobitkilit?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnobitkilit entity);
        Task UpdateAsync(Etnobitkilit entity);
        Task DeleteAsync(int id);
    }
}
