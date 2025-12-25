using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IIlceRepository
    {
        Task<IEnumerable<Ilce>> GetAllAsync();
    }
}
