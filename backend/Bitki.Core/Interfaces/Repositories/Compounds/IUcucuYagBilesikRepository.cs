using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface IUcucuYagBilesikRepository
    {
        Task<IEnumerable<UcucuYagBilesik>> GetAllAsync();
    }
}
