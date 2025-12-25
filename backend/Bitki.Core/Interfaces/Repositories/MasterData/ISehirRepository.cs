using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface ISehirRepository
    {
        Task<IEnumerable<Sehir>> GetAllAsync();
    }
}
