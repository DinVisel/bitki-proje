using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IUcuyagRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Ucuyag>> GetAllAsync();
        Task<FilterResponse<Bitki.Core.Entities.Ucuyag>> QueryAsync(FilterRequest request);
    }
}
