using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturHatalariRepository
    {
        Task<IEnumerable<LiteraturHatalari>> GetAllAsync();
    }
}
