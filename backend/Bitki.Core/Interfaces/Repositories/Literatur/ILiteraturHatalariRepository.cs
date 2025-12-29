using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturHatalariRepository
    {
        Task<IEnumerable<LiteraturHatalari>> GetAllAsync();
        Task<LiteraturHatalari?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturHatalari entity);
        Task UpdateAsync(LiteraturHatalari entity);
        Task DeleteAsync(int id);
    }
}

