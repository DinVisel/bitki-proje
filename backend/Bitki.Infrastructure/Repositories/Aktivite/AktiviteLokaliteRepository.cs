using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Aktivite;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Aktivite
{
    public class AktiviteLokaliteRepository : IAktiviteLokaliteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AktiviteLokaliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AktiviteLokalite>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, yereladi AS LocalName, mevki AS Location, sehirno AS CityId, ilceno AS DistrictId FROM dbo.aktivitelokalite ORDER BY yereladi LIMIT 1000";
            return await connection.QueryAsync<AktiviteLokalite>(sql);
        }
    }
}
