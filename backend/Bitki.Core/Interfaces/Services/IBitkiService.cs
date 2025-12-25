using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Services
{
    public interface IBitkiService
    {
        Task<IEnumerable<Plant>> GetAllPlantsAsync();
        Task<Plant?> GetPlantByIdAsync(int id);
        Task<int> CreatePlantAsync(Plant plant);
        Task UpdatePlantAsync(Plant plant);
        Task DeletePlantAsync(int id);
    }
}
