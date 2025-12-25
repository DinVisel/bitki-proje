using System.Data;
using Bitki.Core.Entities;
using Bitki.Core.Interfaces;
using Bitki.Core.Interfaces.Repositories.Compounds;
using Dapper;

namespace Bitki.Infrastructure.Repositories.Compounds
{
    public class BitkiBilesikRepository : IBitkiBilesikRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BitkiBilesikRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<BitkiBilesik>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = "SELECT id AS Id, bitkino AS PlantId, bilesikno AS CompoundId, miktar AS Amount, aciklama AS Description FROM dbo.bitkibilesik ORDER BY id DESC LIMIT 1000";
            return await connection.QueryAsync<BitkiBilesik>(sql);
        }
    }
}
