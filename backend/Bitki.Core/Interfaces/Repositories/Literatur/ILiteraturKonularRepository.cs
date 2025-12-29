using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Literatur
{
    public interface ILiteraturKonularRepository
    {
        Task<IEnumerable<LiteraturKonular>> GetAllAsync();
        Task<LiteraturKonular?> GetByIdAsync(int id);
        Task<int> AddAsync(LiteraturKonular entity);
        Task UpdateAsync(LiteraturKonular entity);
        Task DeleteAsync(int id);
    }
}

