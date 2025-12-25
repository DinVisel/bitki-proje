using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteRepository : IAktiviteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
