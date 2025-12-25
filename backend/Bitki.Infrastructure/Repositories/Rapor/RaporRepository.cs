using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Rapor;

namespace Bitki.Infrastructure.Repositories.Rapor
{
    public class RaporRepository : IRaporRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RaporRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
