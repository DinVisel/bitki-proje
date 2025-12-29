using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface ISehirRepository
    {
        Task<IEnumerable<Sehir>> GetAllAsync();
        Task<FilterResponse<Sehir>> QueryAsync(FilterRequest request);
        Task<Sehir?> GetByIdAsync(int id);
        Task<int> AddAsync(Sehir entity);
        Task UpdateAsync(Sehir entity);
        Task DeleteAsync(int id);
    }
}

