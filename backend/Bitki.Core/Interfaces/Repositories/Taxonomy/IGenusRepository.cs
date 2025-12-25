using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Taxonomy
{
    public interface IGenusRepository
    {
        Task<IEnumerable<Genus>> GetAllAsync();
        Task<FilterResponse<Genus>> QueryAsync(FilterRequest request);
    }
}
