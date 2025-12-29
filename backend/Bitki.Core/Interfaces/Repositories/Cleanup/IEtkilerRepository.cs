using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IEtkilerRepository
    {
        Task<IEnumerable<Etkiler>> GetAllAsync();
        Task<Etkiler?> GetByIdAsync(int id);
        Task<int> AddAsync(Etkiler entity);
        Task UpdateAsync(Etkiler entity);
        Task DeleteAsync(int id);
    }
}

