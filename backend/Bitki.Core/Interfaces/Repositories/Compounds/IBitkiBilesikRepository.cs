using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBitkiBilesikRepository
    {
        Task<IEnumerable<BitkiBilesik>> GetAllAsync();
    }
}
