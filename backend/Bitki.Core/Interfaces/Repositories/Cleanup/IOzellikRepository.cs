using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Cleanup
{
    public interface IOzellikRepository
    {
        Task<IEnumerable<Bitki.Core.Entities.Ozellik>> GetAllAsync();
    }
}
