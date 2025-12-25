using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IBilesiklerRepository
    {
        Task<IEnumerable<Bilesikler>> GetAllAsync();
    }
}
