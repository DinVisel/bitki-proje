using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBilesiklerRepository
    {
        Task<IEnumerable<Bilesikler>> GetAllAsync();
        Task<FilterResponse<Bilesikler>> QueryAsync(FilterRequest request);
        Task<Bilesikler?> GetByIdAsync(long id);
        Task<long> AddAsync(Bilesikler entity);
        Task UpdateAsync(Bilesikler entity);
        Task DeleteAsync(long id);
    }
}

