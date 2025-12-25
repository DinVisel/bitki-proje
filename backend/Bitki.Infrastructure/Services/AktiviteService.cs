using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Aktivite;

namespace Bitki.Infrastructure.Services
{
    public class AktiviteService : IAktiviteService
    {
        private readonly IAktiviteRepository _activityRepository;

        public AktiviteService(IAktiviteRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
    }
}
