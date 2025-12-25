using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Auth;

namespace Bitki.Infrastructure.Repositories.Auth
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AuthUserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
