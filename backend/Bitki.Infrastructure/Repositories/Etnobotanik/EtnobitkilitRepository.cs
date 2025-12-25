using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnobitkilitRepository : IEtnobitkilitRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EtnobitkilitRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
