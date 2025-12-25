using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Compounds
{
    public interface ILiteraturBilesikRepository
    {
        Task<IEnumerable<LiteraturBilesik>> GetAllAsync();
    }
}
