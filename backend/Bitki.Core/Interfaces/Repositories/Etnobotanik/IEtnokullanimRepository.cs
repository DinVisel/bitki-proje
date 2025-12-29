using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnokullanimRepository
    {
        Task<IEnumerable<Etnokullanim>> GetAllAsync();
        Task<Etnokullanim?> GetByIdAsync(int id);
        Task<int> AddAsync(Etnokullanim entity);
        Task UpdateAsync(Etnokullanim entity);
        Task DeleteAsync(int id);
    }
}
