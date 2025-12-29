using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface IOtorRepository
    {
        Task<IEnumerable<Otor>> GetAllAsync();
        Task<Otor?> GetByIdAsync(long id);
        Task<long> AddAsync(Otor entity);
        Task UpdateAsync(Otor entity);
        Task DeleteAsync(long id);
    }
}

