using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Etnobotanik
{
    public interface IEtnokullanimRepository
    {
        Task<IEnumerable<Etnokullanim>> GetAllAsync();
    }
}
