using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKisilerRepository
    {
        Task<IEnumerable<Kisiler>> GetAllAsync();
        Task<Kisiler?> GetByIdAsync(long id);
        Task<long> AddAsync(Kisiler entity);
        Task UpdateAsync(Kisiler entity);
        Task DeleteAsync(long id);
    }
}

