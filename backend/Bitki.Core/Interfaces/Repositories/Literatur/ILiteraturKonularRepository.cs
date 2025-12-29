using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturKonularRepository
    {
        Task<IEnumerable<LiteraturKonular>> GetAllAsync();
        Task<FilterResponse<LiteraturKonular>> QueryAsync(FilterRequest request);
        Task<LiteraturKonular?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturKonular entity);
        Task UpdateAsync(LiteraturKonular entity);
        Task DeleteAsync(int id);
    }
}

