using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Literatur>> GetAllAsync();
        Task<FilterResponse<Bitki.Core.Entities.Literatur>> QueryAsync(FilterRequest request);
    }
}
