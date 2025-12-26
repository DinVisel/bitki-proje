using Bitki.Core.Entities;
using Bitki.Core.DTOs;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories
{
    public interface IBitkiRepository
    {
        Task<IEnumerable<Plant>> GetAllAsync();
        Task<Plant?> GetByIdAsync(int id);
        Task<BitkiDetailDto?> GetDetailByIdAsync(int id);
        Task<int> AddAsync(Plant plant);
        Task UpdateAsync(Plant plant);
        Task DeleteAsync(int id);
        Task<FilterResponse<Plant>> QueryAsync(FilterRequest request);
    }
}
