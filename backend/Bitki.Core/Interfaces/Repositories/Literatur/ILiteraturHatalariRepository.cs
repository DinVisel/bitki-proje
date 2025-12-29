using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturHatalariRepository
    {
        Task<IEnumerable<LiteraturHatalari>> GetAllAsync();
        Task<FilterResponse<LiteraturHatalari>> QueryAsync(FilterRequest request);
        Task<LiteraturHatalari?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturHatalari entity);
        Task UpdateAsync(LiteraturHatalari entity);
        Task DeleteAsync(int id);
    }
}

