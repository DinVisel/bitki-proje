using System.Data;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.MasterData;

namespace Bitki.Infrastructure.Repositories.MasterData
{
    public class UlkeRepository : IUlkeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UlkeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
