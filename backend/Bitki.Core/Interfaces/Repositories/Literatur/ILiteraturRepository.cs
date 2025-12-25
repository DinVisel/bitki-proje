using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Literatur>> GetAllAsync();
    }
}
