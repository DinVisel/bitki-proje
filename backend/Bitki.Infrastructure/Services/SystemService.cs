using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.System;

namespace Bitki.Infrastructure.Services
{
    public class SystemService : ISystemService
    {
        private readonly IDjangoAdminLogRepository _logRepository;

        public SystemService(IDjangoAdminLogRepository logRepository)
        {
            _logRepository = logRepository;
        }
    }
}
