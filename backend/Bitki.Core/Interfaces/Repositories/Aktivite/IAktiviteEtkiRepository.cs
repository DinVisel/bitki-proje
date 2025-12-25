using Bitki.Core.Entities;

namespace Bitki.Core.Interfaces.Repositories.Aktivite
{
    public interface IAktiviteEtkiRepository
    {
        Task<IEnumerable<AktiviteEtki>> GetAllAsync();
    }
}
