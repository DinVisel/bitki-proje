using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnolokaliteRepository
    {
        Task<IEnumerable<Etnolokalite>> GetAllAsync();
    }
}
