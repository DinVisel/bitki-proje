using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKisilerRepository
    {
        Task<IEnumerable<Kisiler>> GetAllAsync();
    }
}
