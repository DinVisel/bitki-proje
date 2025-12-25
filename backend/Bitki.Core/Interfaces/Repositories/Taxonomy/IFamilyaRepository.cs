using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IFamilyaRepository
    {
        Task<IEnumerable<Familya>> GetAllAsync();
    }
}
