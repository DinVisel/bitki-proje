using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBilesiklerRepository
    {
        Task<IEnumerable<Bilesikler>> GetAllAsync();
        Task<Bilesikler?> GetByIdAsync(long id);
        Task<long> AddAsync(Bilesikler entity);
        Task UpdateAsync(Bilesikler entity);
        Task DeleteAsync(long id);
    }
}

