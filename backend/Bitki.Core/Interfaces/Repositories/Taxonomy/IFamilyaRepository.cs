using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IFamilyaRepository
    {
        Task<IEnumerable<Familya>> GetAllAsync();
        Task<FilterResponse<Familya>> QueryAsync(FilterRequest request);
    }
}
