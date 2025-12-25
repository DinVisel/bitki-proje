using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IUlkeRepository
    {
        Task<IEnumerable<Ulke>> GetAllAsync();
    }
}
