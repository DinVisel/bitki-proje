using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IIlceRepository
    {
        Task<IEnumerable<Ilce>> GetAllAsync();
        Task<FilterResponse<Ilce>> QueryAsync(FilterRequest request);
        Task<Ilce?> GetByIdAsync(int id);
        Task<int> AddAsync(Ilce entity);
        Task UpdateAsync(Ilce entity);
        Task DeleteAsync(int id);
    }
}

