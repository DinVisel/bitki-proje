using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKisilerRepository
    {
        Task<IEnumerable<Kisiler>> GetAllAsync();
        Task<FilterResponse<Kisiler>> QueryAsync(FilterRequest request);
        Task<Kisiler?> GetByIdAsync(long id);
        Task<long> AddAsync(Kisiler entity);
        Task UpdateAsync(Kisiler entity);
        Task DeleteAsync(long id);
    }
}

