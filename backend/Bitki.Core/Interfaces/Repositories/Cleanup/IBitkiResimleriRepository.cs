using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IBitkiResimleriRepository
    {
        Task<IEnumerable<BitkiResimleri>> GetAllAsync();
        Task<FilterResponse<BitkiResimleri>> QueryAsync(FilterRequest request);
        Task<IEnumerable<BitkiResimleri>> GetByPlantIdAsync(int plantId);
        Task<BitkiResimleri?> GetByIdAsync(int id);
        Task<int> AddAsync(BitkiResimleri entity);
        Task UpdateAsync(BitkiResimleri entity);
        Task DeleteAsync(int id);
    }
}

