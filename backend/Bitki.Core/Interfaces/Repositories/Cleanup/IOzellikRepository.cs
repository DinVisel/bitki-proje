using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IOzellikRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Ozellik>> GetAllAsync();
        Task<FilterResponse<Bitki.Core.Entities.Ozellik>> QueryAsync(FilterRequest request);
        Task<Bitki.Core.Entities.Ozellik?> GetByIdAsync(int id);
        Task<int> AddAsync(Bitki.Core.Entities.Ozellik entity);
        Task UpdateAsync(Bitki.Core.Entities.Ozellik entity);
        Task DeleteAsync(int id);
    }
}

