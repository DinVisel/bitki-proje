using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnokullanimRepository
    {
        Task<IEnumerable<Etnokullanim>> GetAllAsync();
        Task<FilterResponse<Etnokullanim>> QueryAsync(FilterRequest request);
        Task<Etnokullanim?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnokullanim entity);
        Task UpdateAsync(Etnokullanim entity);
        Task DeleteAsync(int id);
    }
}
