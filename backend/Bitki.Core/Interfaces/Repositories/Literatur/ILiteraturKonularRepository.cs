using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturKonularRepository
    {
        Task<IEnumerable<LiteraturKonular>> GetAllAsync();
    }
}
