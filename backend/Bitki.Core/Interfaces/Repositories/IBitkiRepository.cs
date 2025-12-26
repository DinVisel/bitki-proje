using Bitki.Core.Entities;
using Bitki.Core.DTOs;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories
{
    public interface IBitkiRepository
    {
        Task<IEnumerable<Plant>> GetAllAsync();
        Task<Plant?> GetByIdAsync(int id);

        // Full detail (all related data - original method)
        Task<BitkiDetailDto?> GetDetailByIdAsync(int id);

        // Lazy loading - basic details only (no related data)
        Task<BitkiDetailDto?> GetBasicDetailByIdAsync(int id);

        // Lazy loading - related data
        Task<IEnumerable<PlantCompoundDto>> GetCompoundsByIdAsync(int id);
        Task<IEnumerable<PlantImageDto>> GetImagesByIdAsync(int id);
        Task<IEnumerable<PlantLiteratureDto>> GetLiteratureByIdAsync(int id);

        Task<int> AddAsync(Plant plant);
        Task UpdateAsync(Plant plant);
        Task DeleteAsync(int id);
        Task<FilterResponse<Plant>> QueryAsync(FilterRequest request);
    }
}
