using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteTestYeriRepository
    {
        Task<IEnumerable<AktiviteTestYeri>> GetAllAsync();
    }
}
