using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteYontemRepository
    {
        Task<IEnumerable<AktiviteYontem>> GetAllAsync();
    }
}
