using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IUlkeRepository
    {
        Task<IEnumerable<Ulke>> GetAllAsync();
        Task<FilterResponse<Ulke>> QueryAsync(FilterRequest request);
        Task<Ulke?> GetByIdAsync(int id);
        Task<int> AddAsync(Ulke entity);
        Task UpdateAsync(Ulke entity);
        Task DeleteAsync(int id);
    }
}

