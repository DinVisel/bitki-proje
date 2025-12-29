using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IUcucuYagBilesikRepository
    {
        Task<IEnumerable<UcucuYagBilesik>> GetAllAsync();
        Task<FilterResponse<UcucuYagBilesik>> QueryAsync(FilterRequest request);
        Task<UcucuYagBilesik?> GetByIdAsync(int essentialOilId, int compoundId);
        Task AddAsync(UcucuYagBilesik entity);
        Task UpdateAsync(UcucuYagBilesik entity);
        Task DeleteAsync(int essentialOilId, int compoundId);
    }
}

