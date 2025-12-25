using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnobitkilitRepository
    {
        Task<IEnumerable<Etnobitkilit>> GetAllAsync();
    }
}
