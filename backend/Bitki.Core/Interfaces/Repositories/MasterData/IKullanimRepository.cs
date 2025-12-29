using Bitki.Core.Entities;
using Bitki.Core.Models;

namespace Bitki.Core.Interfaces.Repositories.MasterData
{
    public interface IKullanimRepository
    {
        Task<IEnumerable<Kullanim>> GetAllAsync();
        Task<FilterResponse<Kullanim>> QueryAsync(FilterRequest request);
        Task<Kullanim?> GetByIdAsync(int id);
        Task<int> AddAsync(Kullanim entity);
        Task UpdateAsync(Kullanim entity);
        Task DeleteAsync(int id);
    }
}

