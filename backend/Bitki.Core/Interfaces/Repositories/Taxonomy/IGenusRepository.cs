using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IGenusRepository
    {
        Task<IEnumerable<Genus>> GetAllAsync();
    }
}
