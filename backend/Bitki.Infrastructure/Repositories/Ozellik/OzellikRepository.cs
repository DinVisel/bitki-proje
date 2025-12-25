using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Ozellik;

namespace Bitki.Infrastructure.Repositories.Ozellik
{
    public class OzellikRepository : IOzellikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public OzellikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
