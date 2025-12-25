using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKullanimRepository
    {
        Task<IEnumerable<Kullanim>> GetAllAsync();
    }
}
