using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IBitkiResimleriRepository
    {
        Task<IEnumerable<BitkiResimleri>> GetAllAsync();
        Task<IEnumerable<BitkiResimleri>> GetByPlantIdAsync(int plantId);
    }
}
