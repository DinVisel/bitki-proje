using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Etnobotanik;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Etnobotanik
{
    public class EtnolokaliteRepository : IEtnolokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EtnolokaliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Etnolokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, yereladi AS LocalName, mevki AS Location, kullanim AS Usage, icerik AS Content FROM dbo.etnolokalite LIMIT 1000";
            return await connection.QueryAsync<Etnolokalite>(sql);
        }
    }
}
