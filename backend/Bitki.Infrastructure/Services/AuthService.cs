using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Auth;

namespace Bitki.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthUserRepository _userRepository;

        public AuthService(IAuthUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
